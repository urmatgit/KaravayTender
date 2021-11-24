// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Razor.Infrastructure.Persistence.Configurations
{
    public class ComParticipantConfiguration : IEntityTypeConfiguration<ComParticipant>
    {

        public void Configure(EntityTypeBuilder<ComParticipant> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(dc => new { dc.ComOfferId, dc.ContragentId });

            //  builder.HasKey(tp => new { tp.UserId, tp.InterestId });

            //builder.HasOne(i => i.Interest)
            //    .WithMany(i => i.UserInterests)
            //    .HasForeignKey(ui => ui.InterestId);
            //builder.HasOne(u => u.ApplicationUser)
            //    .WithMany(u => u.UserInterests)
            //    .HasForeignKey(ui => ui.UserId);

            builder.HasOne(d => d.ComOffer)
                .WithMany(d => d.ComParticipants)
                .HasForeignKey(d => d.ComOfferId)
                .OnDelete(DeleteBehavior.ClientCascade);


            builder.HasOne(c => c.Contragent)
                .WithMany(c => c.ComParticipants)
                .HasForeignKey(c => c.ContragentId)
                .OnDelete(DeleteBehavior.ClientCascade);


        }
    }
}
