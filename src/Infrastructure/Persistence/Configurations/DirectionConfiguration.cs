// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class DirectionConfiguration : IEntityTypeConfiguration<Direction>
    {
        public void Configure(EntityTypeBuilder<Direction> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            builder.Property(t => t.Name)
                .HasMaxLength(50);


        }
    }
}
