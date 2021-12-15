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
    public class StageParticipant : AuditableSoftDeleteEntity, IAuditTrial
    {
        public int ComStageId { get; set; }
        public virtual ComStage ComStage {get;set;}
        [Required]
        public int ContragentId { get; set; }
        public virtual Contragent Contragent { get; set; }
        
        [Required]
        public int ComOfferId { get; set; }
        public virtual ComOffer ComOffer { get; set; }
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new();

    }
}
