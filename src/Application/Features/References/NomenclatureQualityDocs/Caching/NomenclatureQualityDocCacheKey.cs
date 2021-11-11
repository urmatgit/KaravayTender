// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Caching
{
    public static class NomenclatureQualityDocCacheKey
    {
        public const string GetAllCacheKey = "all-NomenclatureQualityDocs";
        public static string GetPagtionCacheKey(string parameters) {
            return "NomenclatureQualityDocsWithPaginationQuery,{parameters}";
        }
    }
}
