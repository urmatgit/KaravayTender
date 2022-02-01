// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

        public static Task<PaginatedData<TDestination>> PaginatedDataAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
                => PaginatedData<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
        public static Task<PaginatedData<TDestination>> PaginatedDataLazySortAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize,string sort,string order)
                => PaginatedData<TDestination>.CreateAsync(queryable, pageNumber, pageSize,sort,order);
        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)

                => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }
}
