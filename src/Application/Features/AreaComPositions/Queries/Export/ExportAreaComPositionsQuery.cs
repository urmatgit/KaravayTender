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
using CleanArchitecture.Razor.Application.Features.AreaComPositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.AreaComPositions.Queries.Export
{
    public class ExportAreaComPositionsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportAreaComPositionsQueryHandler :
         IRequestHandler<ExportAreaComPositionsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportAreaComPositionsQueryHandler> _localizer;

        public ExportAreaComPositionsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportAreaComPositionsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportAreaComPositionsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportAreaComPositionsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<AreaComPosition>(request.FilterRules);
            var data = await _context.AreaComPositions.Where(filters)
                       .OrderBy($"{request.Sort} {request.Order}")
                       .ProjectTo<AreaComPositionDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<AreaComPositionDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["AreaComPositions"]);
            return result;
        }
    }
}

