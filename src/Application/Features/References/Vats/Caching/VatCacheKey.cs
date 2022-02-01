// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Caching
{
    public static class VatCacheKey
    {
        public const string GetAllCacheKey = "all-Vats";
        public static string GetPagtionCacheKey(string parameters)
        {
            return "VatsWithPaginationQuery,{parameters}";
        }
    }
}
