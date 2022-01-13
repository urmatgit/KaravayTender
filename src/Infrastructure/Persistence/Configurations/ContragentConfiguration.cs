// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Infrastructure.Persistence.Configurations
{
    public class ContragentConfiguration : IEntityTypeConfiguration<Contragent>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Contragent> builder)
        {
            builder.Ignore(c => c.DomainEvents);

            //builder.Property(c => c.Id)
            //      .ValueGeneratedNever()
            //      .IsRequired();
            builder.Property(c => c.Name)
                .HasMaxLength(50);


            
            builder.HasOne(a => a.ApplicationUser)
                .WithOne(a => a.Contragent)
                .HasForeignKey<Contragent>(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);

            builder.HasOne(m => m.Manager)

                .WithMany(a => a.MyContragents)
                .HasForeignKey(m => m.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);





        }
    }
}
