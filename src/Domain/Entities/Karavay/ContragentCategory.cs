// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities

{
    public class ContragentCategory : AuditableEntity, IHasDomainEvent, IAuditTrial
    {

        public int ContragentId { get; set; }
        public virtual Contragent Contragent { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
