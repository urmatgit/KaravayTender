// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Models
{
    public class PaginatedData<T>
    {
        public int total { get; set; }
        public IEnumerable<T> rows { get; set; }
        public bool IsSorted { get; set; }
        public PaginatedData(IEnumerable<T> items, int total)
        {
            this.rows = items;
            this.total = total;
        }
        public static PaginatedData<T> CreateWithCheckSort(IEnumerable<T> items,int total, bool IsSorted, string sort, string order)
        {
            if (IsSorted)
                return new PaginatedData<T>(items, total); 
            else
                return new PaginatedData<T>(items.OrderByDynamic(sort, order).Item1,total);
        }
        public PaginatedData(IEnumerable<T> items, int total, string sort, string order)
        {
            if (string.IsNullOrEmpty(sort))
                this.rows = items;
            else
                this.rows = items.OrderByDynamic(sort,order).Item1;
            this.total = total;
        }
        public static async Task<PaginatedData<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var result= new PaginatedData<T>(items, count);
            result.IsSorted= OrderingMethodFinder.OrderMethodExists(source.Expression);
            return result;
        }
        public static async Task<PaginatedData<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize,string sort,string order)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var sorted = OrderingMethodFinder.OrderMethodExists(source.Expression);
            if (!sorted && !string.IsNullOrEmpty(sort))
            {
                var dynamicSort = items.OrderByDynamic(sort, order);
                var result = new PaginatedData<T>(dynamicSort.Item1, count);
                result.IsSorted = dynamicSort.Item2;
                return result;

            }
            else {
                var pageData= new PaginatedData<T>(items, count);
                pageData.IsSorted = true;
                return pageData;
            }
        }
    }
}
