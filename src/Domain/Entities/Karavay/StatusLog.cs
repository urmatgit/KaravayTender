// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Enums;

namespace CleanArchitecture.Razor.Domain.Entities.Karavay
{
   public class StatusLog: AuditableEntity, IHasDomainEvent, IAuditTrial    {
        public int Id { get; set; }
        public int ContragentId { get; set; }
        public virtual Contragent Contragent { get; set; }
        public DateTime DateTime { get; set; }
        public ContragentStatus Status { get; set; }
        public string ApplicationUserId { get; set; }
        public List<DomainEvent> DomainEvents { get; set ; } = new();
    }
}
