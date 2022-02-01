// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Caching
{
    public static class UnitOfCacheKey
    {
        public const string GetAllCacheKey = "all-UnitOfs";
        public static string GetPagtionCacheKey(string parameters)
        {
            return "UnitOfsWithPaginationQuery,{parameters}";
        }
    }
}
