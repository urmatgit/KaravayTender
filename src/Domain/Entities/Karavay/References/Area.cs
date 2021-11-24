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
    public class Area : BaseAuditableEntity, IHasDomainEvent, IAuditTrial //  BaseAuditableEntity,  IAuditTrial
    {
       
        [Required]
        public string Address { get; set; }
        public string Description { get; set; }
        public virtual ICollection<AreaComPosition> AreaComPositions { get; set; }
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
