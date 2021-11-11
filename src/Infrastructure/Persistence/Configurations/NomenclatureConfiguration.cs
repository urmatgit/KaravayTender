// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Infrastructure.Persistence.Configurations
{
    public class NomenclatureConfiguration : IEntityTypeConfiguration<Nomenclature>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Nomenclature> builder)
        {
            builder.Ignore(c => c.DomainEvents);

            builder.Property(c => c.Volume)
                  .IsRequired();
            builder.Property(c => c.DirectionId)
                  .IsRequired();
            builder.Property(c => c.CategoryId)
                  .IsRequired();

            builder.Property(c => c.Name)
                .HasMaxLength(50);
            builder.Property(c => c.UnitOfId)
                  .IsRequired();
            builder.Property(c => c.VatId)
                  .IsRequired();
            



        }
    }
}
