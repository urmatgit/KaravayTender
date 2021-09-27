// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities
{
    public class Contragent : AuditableEntity, IHasDomainEvent, IAuditTrial
    {
        public string ApplicationUserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

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
        [Required]
        [MaxLength(30)]
        public string ContactPhone { get; set; }
        /// <summary>
        /// Контакное лицо
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// вид деятельности
        /// </summary>
        public string TypeOfActivity { get; set; }
        /// <summary>
        /// Услуга
        /// </summary>
        public bool IsService { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        //public string RegistrationNumber { get; set; }
        //public string Password { get; set; }

        public List<DomainEvent> DomainEvents { get; set; }
    }
}
