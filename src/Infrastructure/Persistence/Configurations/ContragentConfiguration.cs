// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Infrastructure.Persistence.Configurations
{
    public class ContragentConfiguration : IEntityTypeConfiguration<Contragent>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Contragent> builder)
        {
            builder.Ignore(c => c.DomainEvents);
            builder.HasKey(c => c.ApplicationUserId);
                    
            builder.Property(c => c.ApplicationUserId)
                  .ValueGeneratedNever()
                  .IsRequired();
            builder.Property(c => c.Name)
                .HasMaxLength(50);



            
        }
    }
}
