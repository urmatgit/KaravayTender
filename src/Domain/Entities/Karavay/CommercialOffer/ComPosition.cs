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
    /// <summary>
    /// Позиции КП
    /// </summary>
    public class ComPosition : AuditableSoftDeleteEntity, IAuditTrial, IHasDomainEvent
    {

        public int Id { get; set; }
        /// <summary>
        /// Количество Поставок в месяц 
        /// </summary>
        [Required]
        public int DeliveryCount { get; set; }
        /// <summary>
        /// Объем  поставок по контракту
        /// </summary>
        [Required]
        public decimal Volume { get; set; }
        public string AddRequirement { get; set; }

        /// <summary>
        /// Цена без ндс
        /// </summary>
        [Required]
        public decimal Price { get; set; }
        /// <summary>
        /// Сумма без НДС
        /// </summary>
        [Required]
        public decimal Summa { get; set; }
        /// <summary>
        /// Сумма с НДС
        /// </summary>
        [Required]
        public decimal SummaVAT { get; set; }

        [Required]
        public int NomenclatureId { get; set; }
        public virtual Nomenclature Nomenclature { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<AreaComPosition> AreaComPositions { get; set; }
        [Required]
        public int ComOfferId { get; set; }
        public virtual  ComOffer ComOffer { get; set; }
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
