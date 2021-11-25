// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Audit;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Entities.Log;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Logger> Loggers { get; set; }
        DbSet<AuditTrail> AuditTrails { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Product> Products { get; set; }
        //DbSet<DocumentType> DocumentTypes { get; set; }
        //DbSet<Document> Documents { get; set; }
        DbSet<KeyValue> KeyValues { get; set; }
        //DbSet<ApprovalData> ApprovalDatas { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Direction> Directions { get; set; }
        DbSet<Contragent> Contragents { get; set; }

        DbSet<ContragentCategory> ContragentCategories { get; set; }
        DbSet<StatusLog> StatusLogs { get; set; }
        public DbSet<UnitOf> UnitOfs { get; set; }
        public DbSet<Vat> Vats { get; set; }
        public DbSet<QualityDoc> QualityDocs { get; set; }
        public DbSet<Nomenclature> Nomenclatures { get; set; }
        public DbSet<NomenclatureQualityDoc> NomenclatureQualityDocs { get; set; }
        public DbSet<Area> Areas { get; set; }

        public DbSet<ComOffer> ComOffers { get; set; }
        public DbSet<ComParticipant> ComParticipants { get; set; }
        public DbSet<ComStage> ComStages { get; set; }
        public DbSet<ComPosition> ComPositions { get; set; }
        public DbSet<StageComposition> StageCompositions { get; set; }
        public DbSet<AreaComPosition> AreaComPositions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
