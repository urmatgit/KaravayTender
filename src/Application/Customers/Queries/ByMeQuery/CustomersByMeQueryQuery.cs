// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Features.Customers.DTOs;
using CleanArchitecture.Razor.Application.Common.Specification;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using CleanArchitecture.Razor.Application.Features.Customers.Caching;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Application.Models;
using System.Threading.Tasks;
using System.Threading;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using CleanArchitecture.Razor.Application.Common.Interfaces.Caching;
using CleanArchitecture.Razor.Application.Common.Models;
using System.Linq;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Mappings;

namespace CleanArchitecture.Razor.Application.Features.Customers.Queries.PaginationQuery
{

    public class CustomersByMeQueryQuery : PaginationRequest, IRequest<PaginatedData<CustomerDto>>, ICacheable
    {
        public string UserId { get; set; }


        public string CacheKey => $"{nameof(CustomersByMeQueryQuery)},userid:{UserId},{this.ToString()}";

        public MemoryCacheEntryOptions Options => CustomerCacheKey.MemoryCacheEntryOptions;
    }

    public class ByMeCustomersQueryHandler : IRequestHandler<CustomersByMeQueryQuery, PaginatedData<CustomerDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ByMeCustomersQueryHandler(

            IApplicationDbContext context,
            IMapper mapper
            )
        {

            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedData<CustomerDto>> Handle(CustomersByMeQueryQuery request, CancellationToken cancellationToken)
        {
            var filters = PredicateBuilder.FromFilter<Customer>(request.FilterRules);
            var data = await _context.Customers.Specify(new CustomerByMeQuerySpec(request.UserId))
                .Where(filters)
                //.OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);

            return data;
        }

        public class CustomerByMeQuerySpec : Specification<Customer>
        {
            public CustomerByMeQuerySpec(string userId)
            {
                Criteria = p => p.CreatedBy == userId;
            }


        }
    }
}
