// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Contragents.Caching
{
    public static class ContragentCacheKey
    {
        public const string GetAllCacheKey = "all-Contragents";
        public static string GetPagtionCacheKey(string parameters)
        {
            return "ContragentsWithPaginationQuery,{parameters}";
        }
    }
}
