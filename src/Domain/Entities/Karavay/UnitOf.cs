// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities.Karavay
{
    /// <summary>
    /// Ед. измерения,  the entity why called "UnitOf", because the word "Unit"   using in Mediator class
    /// </summary>
    public class UnitOf : BaseAuditableEntity, IAuditTrial
    {
        
        public virtual ICollection<Nomenclature> Nomenclatures { get; set; }
    }
}