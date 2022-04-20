// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Caching
{
    public static class AreaCacheKey
    {
        public const string GetAllCacheKey = "all-Areas";
        public static string GetPagtionCacheKey(string parameters) {
            return $"AreasWithPaginationQuery,{parameters}";
        }
       
        static AreaCacheKey()
        {
            ResetCacheToken = new CancellationTokenSource(new TimeSpan(0, 3, 0));
        }
        public static CancellationTokenSource ResetCacheToken { get; private set; }
        public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(ResetCacheToken.Token));
    }
}
