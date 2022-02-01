// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Features.ComStages.DTOs
{
    public class GoodPriceDto
    {
        public int ContragentId { get; set; }
        public decimal? Price { get; set; }
        public string  ColorRGB { get; set; }

    }
}
