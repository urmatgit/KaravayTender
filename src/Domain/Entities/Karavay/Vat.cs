// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities.Karavay
{
    /// <summary>
    /// НДС
    /// </summary>
    public class Vat : BaseAuditableEntity, IAuditTrial
    {


        public decimal Value { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Nomenclature> Nomenclatures { get; set; }
    }
}
