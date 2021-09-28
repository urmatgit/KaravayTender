// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Razor.Application.Common.Extensions
{
  public  static class CacheExntension
    {
        public static string AllCacheKey(this object entity,string additional)
        {
            return $"{entity.GetType().Name}-{additional}";
        }
        public static string GetPagtionCacheKey(this object entity, string parameters)
        {
            return $"{entity.GetType().Name},{parameters}";
        }
    }
}
