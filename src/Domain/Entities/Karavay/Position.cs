// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities.Karavay
{
    public class Position : AuditableSoftDeleteEntity, IAuditTrial, IHasDomainEvent
    {
        public int Id { get; set; }
        /// <summary>
        /// Количество Поставок в месяц 
        /// </summary>
        public int DeliveryCount { get; set; }
        /// <summary>
        /// Объем  поставок по контракту
        /// </summary>
        public decimal Volume { get; set; }
        public string AddRequirement { get; set; }
        [Required]
        public int NomenclatureId { get; set; }
        public virtual Nomenclature Nomenclature { get; set; }

        public int CategoryId { get; set; }
        public  virtual Category Category { get; set; }
        public virtual ICollection<AreaPosition> AreaPositions { get; set; }  
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
