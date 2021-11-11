// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.Features.Directions.Caching
{
    public static class DirectionCacheKey
    {
        public const string GetAllCacheKey = "all-Directions";
        public static string GetPagtionCacheKey(string parameters)
        {
            return $"DirectionsWithPaginationQuery,{parameters}";
        }
    }
}
