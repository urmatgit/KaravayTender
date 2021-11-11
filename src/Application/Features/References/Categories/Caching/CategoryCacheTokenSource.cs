// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;

namespace CleanArchitecture.Razor.Application.Features.Categories.Caching
{
    public sealed class CategoryCacheTokenSource
    {
        static CategoryCacheTokenSource()
        {
            ResetCacheToken = new CancellationTokenSource();
        }
        public static CancellationTokenSource ResetCacheToken { get; private set; }
    }
}
