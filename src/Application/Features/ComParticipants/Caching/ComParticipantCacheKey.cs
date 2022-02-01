// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Caching
{
    public static class ComParticipantCacheKey
    {
        public const string GetAllCacheKey = "all-ComParticipants";
        public static string GetPagtionCacheKey(string parameters) {
            return "ComParticipantsWithPaginationQuery,{parameters}";
        }
    }
}
