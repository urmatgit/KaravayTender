// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities
{
    public class Category : AuditableEntity, IHasDomainEvent, IAuditTrial
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DirectionId { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual ICollection<ContragentCategory> ContragentCategories { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new();

    }
}
