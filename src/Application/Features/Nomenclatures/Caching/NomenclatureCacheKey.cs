﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Caching
{
    public static class NomenclatureCacheKey
    {
        public const string GetAllCacheKey = "all-Nomenclatures";
        public static string GetPagtionCacheKey(string parameters) {
            return "NomenclaturesWithPaginationQuery,{parameters}";
        }
    }
}
