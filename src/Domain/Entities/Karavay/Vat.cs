// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Common;

namespace CleanArchitecture.Razor.Domain.Entities.Karavay
{
    /// <summary>
    /// НДС
    /// </summary>
    public class Vat:  BaseAuditableEntity
    {
        
        public decimal Value { get; set; }
        public virtual ICollection<Nomenclature> Nomenclatures { get; set; }
    }
}
