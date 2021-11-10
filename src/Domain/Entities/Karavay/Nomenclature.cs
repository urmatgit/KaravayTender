// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities.Karavay
{
    public class Nomenclature : BaseAuditableEntity, IAuditTrial//, IHasDomainEvent
    {

        public int DirectionId { get; set; }
        public virtual Direction Direction { get; set; }
        public int UnitId { get; set; }
        public virtual UnitOf Unit {get;set;}
        public int VatId { get; set; }
        public virtual Vat Vat { get; set; }
       // public List<DomainEvent> DomainEvents { get; set; }
    }
}
