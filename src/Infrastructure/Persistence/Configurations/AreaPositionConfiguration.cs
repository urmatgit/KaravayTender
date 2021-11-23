// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Razor.Infrastructure.Persistence.Configurations
{
    public class AreaPositionConfiguration : IEntityTypeConfiguration<AreaPosition>
    {

        public void Configure(EntityTypeBuilder<AreaPosition> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(dc => new { dc.AreaId, dc.PositionId });

            //  builder.HasKey(tp => new { tp.UserId, tp.InterestId });

            //builder.HasOne(i => i.Interest)
            //    .WithMany(i => i.UserInterests)
            //    .HasForeignKey(ui => ui.InterestId);
            //builder.HasOne(u => u.ApplicationUser)
            //    .WithMany(u => u.UserInterests)
            //    .HasForeignKey(ui => ui.UserId);

            builder.HasOne(d => d.Area)
                .WithMany(d => d.AreaPositions)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientCascade);


            builder.HasOne(c => c.Position)
                .WithMany(c => c.AreaPositions)
                .HasForeignKey(c => c.PositionId)
                .OnDelete(DeleteBehavior.ClientCascade);


        }
    }
}
