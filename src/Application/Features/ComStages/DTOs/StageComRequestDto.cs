// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Features.ComStages.DTOs
{
   public class StageComRequestDto
    {
        public int ComOfferId { get; set; }
        public int StageId { get; set; }
        
        public ICollection<ContrPriceDto> ContrPrices { get; set; }
    }
    public class ContrPriceDto
    {
        public int ContrId { get; set; }
        public int ComPositionId { get; set; }
        public bool RequestPrice { get; set; }
    }

}
