// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Razor.Infrastructure.Persistence.Configurations
{
    public class StageCompositionConfiguration : IEntityTypeConfiguration<StageComposition>
    {

        public void Configure(EntityTypeBuilder<StageComposition> builder)
        {
        
            builder.HasKey(dc => new { dc.ComStageId, dc.ContragentId,dc.ComPositionId });

            //  builder.HasKey(tp => new { tp.UserId, tp.InterestId });

            //builder.HasOne(i => i.Interest)
            //    .WithMany(i => i.UserInterests)
            //    .HasForeignKey(ui => ui.InterestId);
            //builder.HasOne(u => u.ApplicationUser)
            //    .WithMany(u => u.UserInterests)
            //    .HasForeignKey(ui => ui.UserId);

            builder.HasOne(d => d.ComStage)
                .WithMany(d => d.StageCompositions)
                .HasForeignKey(d => d.ComStageId)
                .OnDelete(DeleteBehavior.ClientCascade);


            builder.HasOne(c => c.Contragent)
                .WithMany(c => c.StageCompositions)
                .HasForeignKey(c => c.ContragentId)
                .OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(c => c.ComPosition)
               .WithMany(c => c.StageCompositions)
               .HasForeignKey(c => c.ComPositionId)
               .OnDelete(DeleteBehavior.ClientCascade);


        }
    }
}
