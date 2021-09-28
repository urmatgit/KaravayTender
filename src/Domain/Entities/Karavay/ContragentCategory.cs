// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities

{
    public class ContragentCategory : AuditableEntity, IHasDomainEvent, IAuditTrial
    {
        [Column("ApplicationUserId")]
        public string ContragentId { get; set; }
        public virtual Contragent Contragent { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public List<DomainEvent> DomainEvents { get; set; }
    }
}
