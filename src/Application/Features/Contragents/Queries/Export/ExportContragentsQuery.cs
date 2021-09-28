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
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Queries.Export
{
    public class ExportContragentsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportContragentsQueryHandler :
         IRequestHandler<ExportContragentsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportContragentsQueryHandler> _localizer;

        public ExportContragentsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportContragentsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportContragentsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportContragentsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<Contragent>(request.FilterRules);
            var data = await _context.Contragents.Where(filters)
                       .OrderBy("{request.Sort} {request.Order}")
                       .ProjectTo<ContragentDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<ContragentDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["Contragents"]);
            return result;
        }
    }
}

