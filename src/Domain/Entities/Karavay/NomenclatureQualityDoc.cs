// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Domain.Entities

{
    public class NomenclatureQualityDoc : AuditableEntity, IHasDomainEvent, IAuditTrial
    {

        public int NomenclatureId { get; set; }
        public virtual Nomenclature Nomenclature { get; set; }
        public int QualityDocId { get; set; }
        public virtual QualityDoc QualityDoc { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
