// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;

namespace CleanArchitecture.Razor.Application.Common.Interfaces.Caching
{
    public interface ICacheInvalidator
    {
        string CacheKey { get; }
        CancellationTokenSource ResetCacheToken { get; }
    }
}
