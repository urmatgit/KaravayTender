// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Caching
{
    public static class ContragentCategoryCacheKey
    {
        public const string GetAllCacheKey = "all-ContragentCategories";
        public static string GetPagtionCacheKey(string parameters)
        {
            return "ContragentCategoriesWithPaginationQuery,{parameters}";
        }
    }
}
