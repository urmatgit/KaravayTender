// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Entities
{
    public class Direction : AuditableEntity, IHasDomainEvent, IAuditTrial
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        //public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Nomenclature> Nomenclatures { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
