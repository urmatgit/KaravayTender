// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Entities.Karavay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class ComOfferConfiguration : IEntityTypeConfiguration<ComOffer>
    {
        public void Configure(EntityTypeBuilder<ComOffer> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasOne(c => c.Direction)
               .WithMany(d => d.ComOffers)
               .HasForeignKey(d => d.DirectionId)
               .OnDelete(DeleteBehavior.ClientCascade);






        }
    }
}
