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
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Products.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Products.Queries.Pagination
{
    public class ProductsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ProductDto>>
    {
        public string filterRules { get; set; }
        public int page { get; set; } = 1;
        public int rows { get; set; } = 15;
        public string sort { get; set; } = "Id";
        public string order { get; set; } = "desc";

    }

    public class ProductsWithPaginationQueryHandler :
         IRequestHandler<ProductsWithPaginationQuery, PaginatedData<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ProductsWithPaginationQueryHandler> _localizer;

        public ProductsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ProductsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<ProductDto>> Handle(ProductsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ProductsWithPaginationQueryHandler method 
            var filters = PredicateBuilder.FromFilter<Product>(request.filterRules);
            var data = await _context.Products.Where(filters)
                .OrderBy($"{request.sort} {request.order}")
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.page, request.rows);

            return data;
        }
    }
}

