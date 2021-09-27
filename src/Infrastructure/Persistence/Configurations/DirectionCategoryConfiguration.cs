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
    public class DirectionCategoryConfiguration : IEntityTypeConfiguration<DirectionCategory>
    {
        public void Configure1(EntityTypeBuilder<DirectionCategory> builder)
        {
            
        }
        public void Configure(EntityTypeBuilder<DirectionCategory> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            builder.HasKey(dc => new {dc.DirectionId,dc.CategoryId });
            builder.HasOne(d => d.Direction)
                .WithMany(d => d.DirectionCategories)
                .HasForeignKey(d => d.DirectionId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c => c.Category)
                .WithMany(c => c.DirectionCategories)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
