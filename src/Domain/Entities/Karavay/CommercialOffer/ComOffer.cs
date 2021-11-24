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
using CleanArchitecture.Razor.Domain.Enums;

namespace CleanArchitecture.Razor.Domain.Entities.Karavay
{
    public class ComOffer : AuditableSoftDeleteEntity, IAuditTrial, IHasDomainEvent
    {

        public int Id { get; set; }
        

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public LotStatus Status { get; set; } = LotStatus.Preparation;
        /// <summary>
        /// Номер лота
        /// </summary>
        [Required]
        public string Number { get; set; }
        
        [Required]
        public DateTime DateBegin { get; set; }
        [Required]
        public DateTime  DateEnd { get; set; }
        [Required]
        public int DirectionId { get; set; }
        public virtual Direction Direction { get; set; }

        [Required]
        public DateTime TermBegin { get; set; }
        [Required]
        public DateTime TermEnd { get; set; }
        [Required]
        public string ManagerId { get; set; }

        /// <summary>
        /// Отсрочка платежа, дней
        /// </summary>
        public short DelayDay { get; set; }
        /// <summary>
        /// Отсрочка в  Банковских днях
        /// </summary>
        public bool IsBankDays { get; set; }
        

        public int WinnerId { get; set; }
        public virtual Contragent Winner { get; set; }
        /// <summary>
        /// Цена доставки включена в стоимость 
        /// </summary>
        public bool IsPriceInDelivery { get; set; }
        
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new();

        //
    }
}
