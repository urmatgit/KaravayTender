// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Customers.Caching;
using CleanArchitecture.Razor.Application.Customers.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CleanArchitecture.Razor.Application.Customers.Queries.PaginationQuery
{
    public class CustomersWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<CustomerDto>>
    {
        public string CacheKey => CustomerCacheKey.GetPaginationCacheKey(this.ToString());
        public int? ProductId { get; set; }




        public MemoryCacheEntryOptions Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(CustomerCacheTokenSource.ResetCacheToken.Token));

    }
    public class CustomersWithPaginationQueryHandler : IRequestHandler<CustomersWithPaginationQuery, PaginatedData<CustomerDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CustomersWithPaginationQueryHandler(

            IApplicationDbContext context,
            IMapper mapper
            )
        {

            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedData<CustomerDto>> Handle(CustomersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var filters = PredicateBuilder.FromFilter<Customer>(request.FilterRules);
            if (request.ProductId > 0)
            {
                filters = filters.And(p => p.ProductId == request.ProductId);
            }
            var data = await _context.Customers.Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);

            return data;
        }
    }
}
