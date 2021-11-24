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
    public class ComParticipant : AuditableSoftDeleteEntity, IAuditTrial, IHasDomainEvent
    {

        /// <summary>
        /// Этап отказа
        /// </summary>
        public int StepFailure { get; set; }
        public ParticipantStatus Status { get; set; } = ParticipantStatus.NotParticipate;
        [Required]
        public int ContragentId { get; set; }
        public virtual Contragent Contragent { get; set; }
        [Required]
        public int ComOfferId { get; set; }
        public virtual ComOffer ComOffer { get; set; }
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new() ;
    }
}
