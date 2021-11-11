// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities.Karavay
{
    public class Nomenclature : BaseAuditableEntity, IAuditTrial, IHasDomainEvent
    {

         
        /// <summary>
        /// название файла Спецификации
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// Объем Потребление в месяц
        /// </summary>
        public decimal Volume { get; set; }
        public int CategoryId { get; set; }
        public virtual Category  Category { get; set; }
        public int DirectionId { get; set; }
        public virtual Direction Direction { get; set; }
        public int UnitOfId { get; set; }
        public virtual UnitOf Unit { get; set; }
        public int VatId { get; set; }
        public virtual Vat Vat { get; set; }
        public virtual ICollection<NomenclatureQualityDoc> NomenclatureQualityDocs { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
