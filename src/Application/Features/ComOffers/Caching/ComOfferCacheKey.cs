// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Caching
{
    public static class ComOfferCacheKey
    {
        public const string GetAllCacheKey = "all-ComOffers";
        public static string GetPagtionCacheKey(string parameters) {
            return "ComOffersWithPaginationQuery,{parameters}";
        }
    }
}
