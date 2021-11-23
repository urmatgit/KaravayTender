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
    public class Lot : AuditableSoftDeleteEntity, IAuditTrial, IHasDomainEvent
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// Номер лота
        /// </summary>
         [Required]
        public string Number { get; set; }
        [Required]
        public LotStatus Status { get; set; }
        [Required]
        public DateTime DateBegin { get; set; }
        [Required]
        public DateTime  DateEnd { get; set; }
        /// <summary>
        /// Отсрочка платежа, дней
        /// </summary>
        public short DelayDay { get; set; }
        /// <summary>
        /// Отсрочка в  Банковских днях
        /// </summary>
        public bool IsBankDays { get; set; }
        [Required]
        public string ManagerId { get; set; }

        public int WinnerId { get; set; }
        public virtual Contragent Winner { get; set; }

        public bool IsIncludeDelivery { get; set; }
        [Required]
        public int DirectionId { get; set; }
        public virtual Direction Direction { get; set; }
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
