using System;
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
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Queries.Export
{
    public class ExportComPositionsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportComPositionsQueryHandler :
         IRequestHandler<ExportComPositionsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportComPositionsQueryHandler> _localizer;

        public ExportComPositionsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportComPositionsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportComPositionsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportComPositionsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<ComPosition>(request.FilterRules);
            var data = await _context.ComPositions.Where(filters)
                       .OrderBy("{request.Sort} {request.Order}")
                       .ProjectTo<ComPositionDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<ComPositionDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["ComPositions"]);
            return result;
        }
    }
}

