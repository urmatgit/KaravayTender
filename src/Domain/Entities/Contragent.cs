// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities
{
    public class Contragent : AuditableEntity, IHasDomainEvent, IAuditTrial
    {
        public string ApplicationUserId { get; set; }
        
        public string Name { get; set; }

        public List<DomainEvent> DomainEvents { get; set; }
    }
}
