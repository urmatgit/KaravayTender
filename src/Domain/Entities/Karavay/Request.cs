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
    public class Request : AuditableEntity, IHasDomainEvent, IAuditTrial
    {
        public int Id { get; set; }
        public RequestStatus Status { get; set; }
        //public DateTime RequestCreated { get; set; }
        public DateTime Registrated { get; set; }
        public List<DomainEvent> DomainEvents { get; set; }
    }
}
