﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Domain.Entities;
using System.Linq.Dynamic.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Localization;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Queries.Export
{
    public class ExportContragentCategoriesQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportContragentCategoriesQueryHandler :
         IRequestHandler<ExportContragentCategoriesQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportContragentCategoriesQueryHandler> _localizer;

        public ExportContragentCategoriesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportContragentCategoriesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportContragentCategoriesQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportContragentCategoriesQueryHandler method 
            var filters = PredicateBuilder.FromFilter<ContragentCategory>(request.FilterRules);
            var data = await _context.ContragentCategories.Where(filters)
                       .OrderBy("{request.Sort} {request.Order}")
                       .ProjectTo<ContragentCategoryDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<ContragentCategoryDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["ContragentCategories"]);
            return result;
        }
    }
}
