// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Audit;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Entities.Log;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Domain.Identity;
using CleanArchitecture.Razor.Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Razor.Application.Common.Extensions;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data;
using System;

namespace CleanArchitecture.Razor.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser, ApplicationRole, string,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext() : base()
        {
        }
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService,
            IDomainEventService domainEventService,
            IDateTime dateTime
            ) : base(options)
        {
            _currentUserService = currentUserService;
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }
        public DbSet<Logger> Loggers { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<DocumentType> DocumentTypes { get; set; }
        //public DbSet<Document> Documents { get; set; }

        public DbSet<KeyValue> KeyValues { get; set; }
        //  public DbSet<ApprovalData> ApprovalDatas { get; set; }


        public DbSet<Direction> Directions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contragent> Contragents { get; set; }
        public DbSet<ContragentCategory> ContragentCategories { get; set; }
        public DbSet<StatusLog> StatusLogs { get; set; }
        public DbSet<UnitOf> UnitOfs { get; set; }
        public DbSet<Vat> Vats { get; set; }
        public DbSet<QualityDoc> QualityDocs { get; set; }
        public DbSet<Nomenclature> Nomenclatures { get; set; }
        public DbSet<NomenclatureQualityDoc> NomenclatureQualityDocs { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<ComOffer> ComOffers { get; set; }
        public DbSet<ComParticipant> ComParticipants {get;set;}
        public DbSet<ComStage> ComStages { get; set; }
        public DbSet<ComPosition>  ComPositions { get; set; }
        public DbSet<StageComposition> StageCompositions { get; set; }
        public DbSet<AreaComPosition> AreaComPositions { get; set; }
        public DbSet<StageParticipant> StageParticipants { get; set; }
      
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {


            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                    case EntityState.Deleted:
                        if (entry.Entity is ISoftDelete softDelete)
                        {
                            softDelete.DeletedBy = _currentUserService.UserId;
                            softDelete.Deleted = _dateTime.Now;
                            entry.State = EntityState.Modified;
                        }
                        break;
                }
            }

            var events = ChangeTracker.Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .Where(domainEvent => !domainEvent.IsPublished)
                    .ToArray();
            var auditEntries = OnBeforeSaveChanges(_currentUserService.UserId);
            var result = await base.SaveChangesAsync(cancellationToken);
            await DispatchEvents(events);
            await OnAfterSaveChanges(auditEntries, cancellationToken);
            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.ApplyGlobalFilters<ISoftDelete>(s => s.Deleted == null);
        }

        private async Task DispatchEvents(DomainEvent[] events)
        {
            foreach (var @event in events)
            {
                @event.IsPublished = true;
                await _domainEventService.Publish(@event);
            }
        }


        private List<AuditTrail> OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditTrail>();
            foreach (var entry in ChangeTracker.Entries<IAuditTrial>())
            {
                if (entry.Entity is AuditTrail ||
                    entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditTrail()
                {
                    DateTime = _dateTime.Now,
                    TableName = entry.Entity.GetType().Name,
                    UserId = userId,
                    AffectedColumns = new List<string>()
                };
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {

                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.PrimaryKey[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                            {
                                auditEntry.AffectedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                AuditTrails.Add(auditEntry);
            }
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<AuditTrail> auditEntries, CancellationToken cancellationToken = new())
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.PrimaryKey[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                AuditTrails.Add(auditEntry);
            }
            return SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Executes raw query with parameters and maps returned values to column property names of Model provided.
        /// Not all properties are required to be present in model (if not present - null)
        /// </summary>
        public List<T> ExecuteSqlRawExt1<T, P>( string query, Func<DbDataReader, T> map, IEnumerable<P> queryParameters = null)
            
        {
            var result = this.ExecuteSqlRawExt<T,P>(query, map, queryParameters);
            return result;
        }

    }
}
