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
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Directions.Queries.Export
{
    public class ExportDirectionsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportDirectionsQueryHandler :
         IRequestHandler<ExportDirectionsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportDirectionsQueryHandler> _localizer;

        public ExportDirectionsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportDirectionsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportDirectionsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportDirectionsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<Direction>(request.FilterRules);
            var data = await _context.Directions.Where(filters)
                       .OrderBy("{request.Sort} {request.Order}")
                       .ProjectTo<DirectionDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<DirectionDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["Directions"]);
            return result;
        }
    }
}

