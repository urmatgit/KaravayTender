// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Products.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Products.Queries.Export
{
    public class ExportProductsQuery : IRequest<byte[]>
    {
        public string filterRules { get; set; }
        public string sort { get; set; } = "Id";
        public string order { get; set; } = "desc";
    }

    public class ExportProductsQueryHandler :
         IRequestHandler<ExportProductsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportProductsQueryHandler> _localizer;

        public ExportProductsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
               IExcelService excelService,
            IStringLocalizer<ExportProductsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _excelService = excelService;
        }

        public async Task<byte[]> Handle(ExportProductsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportProductsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<Product>(request.filterRules);
            var data = await _context.Products.Where(filters)
                .OrderBy($"{request.sort} {request.order}")
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<ProductDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                    { _localizer["Name"], item => item.Name },
                    { _localizer["Price"], item => item.Price },
                    { _localizer["Comments"], item => item.Description },

                }, _localizer["Products"]
                );
            return result;
        }
    }
}

