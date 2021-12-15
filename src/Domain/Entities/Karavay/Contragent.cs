// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Domain.Identity;

namespace CleanArchitecture.Razor.Domain.Entities
{
    public class Contragent : BaseAuditableEntity, IHasDomainEvent, IAuditTrial
    {



        //[ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }


        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(12)]
        public string INN { get; set; }
        [MaxLength(12)]
        public string KPP { get; set; }
        [MaxLength(30)]
        public string Site { get; set; }
        [Required]
        [MaxLength(30)]
        public string Phone { get; set; }
        /// <summary>
        /// Контакное лицо
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ContactPerson { get; set; }
        [Required]
        [MaxLength(30)]
        public string ContactPhone { get; set; }
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }
        /// <summary>
        /// вид деятельности
        /// </summary>
        public string TypeOfActivity { get; set; }
        /// <summary>
        /// Услуга
        /// </summary>
        public bool IsService { get; set; }
        public string ManagerId { get; set; }
        public virtual ApplicationUser Manager { get; set; }

        public string Files { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        //public string RegistrationNumber { get; set; }
        //public string Password { get; set; }
        public ContragentStatus Status { get; set; } = ContragentStatus.OnRegistration;
        public int DirectionId { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual ICollection<ContragentCategory> ContragentCategories { get; set; }
        public virtual ICollection<StatusLog> StatusLogs { get; set; }
        public virtual ICollection<ComOffer> ComOffers { get; set; }
        public virtual ICollection<ComParticipant> ComParticipants { get; set; }
        public virtual ICollection<StageComposition> StageCompositions { get; set; }
        public virtual ICollection<StageParticipant> StageParticipants { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
