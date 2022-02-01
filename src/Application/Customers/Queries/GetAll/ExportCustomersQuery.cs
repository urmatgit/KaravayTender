// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Caching;
using CleanArchitecture.Razor.Application.Customers.Caching;
using CleanArchitecture.Razor.Application.Customers.DTOs;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Primitives;

namespace CleanArchitecture.Razor.Application.Customers.Queries.GetAll
{
    public class GetAllCustomersQuery : IRequest<IEnumerable<CustomerDto>>, ICacheable
    {
        public string CacheKey => CustomerCacheKey.GetAllCacheKey;

        public MemoryCacheEntryOptions Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(CustomerCacheTokenSource.ResetCacheToken.Token));
    }

    public class GetAllCustomersQueryQueryHandler :
         IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllCustomersQueryQueryHandler> _localizer;

        public GetAllCustomersQueryQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllCustomersQueryQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
