// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities
{
    public class Product : AuditableEntity, IHasDomainEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }


        public virtual List<Customer> Customers { get; set; } = new();
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
