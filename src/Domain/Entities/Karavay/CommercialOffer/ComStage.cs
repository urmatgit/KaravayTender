// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities.Karavay
{
    /// <summary>
    /// Этапы
    /// </summary>
    public class ComStage : AuditableSoftDeleteEntity, IAuditTrial, IHasDomainEvent
    {
        public ComStage()
        {
            

        }
        public int Id { get; set; }
        /// <summary>
        /// Порядковый номер
        /// </summary>
        [Required]
        public int Number { get; set; }
        /// <summary>
        /// Срок предоставления ответа,дни
        /// </summary>
        [Required]
        public int Deadline { get; set; }
        public DateTime DeadlineDate { get; set; }
        public int  ComOfferId { get; set; }
        public  virtual ComOffer ComOffer { get; set; }

        public virtual ICollection<StageComposition> StageCompositions { get; set; }
        public virtual ICollection<StageParticipant> StageParticipants { get; set; }
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
