// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Razor.Infrastructure.Persistence.Configurations
{
    public class ContragentCategoryConfiguration : IEntityTypeConfiguration<ContragentCategory>
    {
       
        public void Configure(EntityTypeBuilder<ContragentCategory> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(dc => new {dc.ContragentId ,dc.CategoryId });

            //  builder.HasKey(tp => new { tp.UserId, tp.InterestId });

            //builder.HasOne(i => i.Interest)
            //    .WithMany(i => i.UserInterests)
            //    .HasForeignKey(ui => ui.InterestId);
            //builder.HasOne(u => u.ApplicationUser)
            //    .WithMany(u => u.UserInterests)
            //    .HasForeignKey(ui => ui.UserId);

            builder.HasOne(d => d.Contragent)
                .WithMany(d => d.ContragentCategories)
                .HasForeignKey(d => d.ContragentId)
                .OnDelete(DeleteBehavior.ClientCascade);


            builder.HasOne(c => c.Category)
                .WithMany(c => c.ContragentCategories)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.ClientCascade);
                
                
        }
    }
}
