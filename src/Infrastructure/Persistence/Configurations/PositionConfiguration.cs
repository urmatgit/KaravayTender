// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Entities.Karavay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<ComPosition>
    {
        public void Configure(EntityTypeBuilder<ComPosition> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            builder.Property(t => t.Volume)
                .IsRequired();

            builder.HasOne(c => c.ComOffer)
             .WithMany(d => d.ComPositions)
             .HasForeignKey(d => d.ComOfferId)
             .OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(c => c.Nomenclature)
             .WithMany(d => d.ComPositions)
             .HasForeignKey(d => d.NomenclatureId)
             .OnDelete(DeleteBehavior.ClientCascade);





        }
    }
}
