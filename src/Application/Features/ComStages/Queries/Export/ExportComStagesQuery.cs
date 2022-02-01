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
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Queries.Export
{
    public class ExportComStagesQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportComStagesQueryHandler :
         IRequestHandler<ExportComStagesQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportComStagesQueryHandler> _localizer;

        public ExportComStagesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportComStagesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportComStagesQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportComStagesQueryHandler method 
            var filters = PredicateBuilder.FromFilter<ComStage>(request.FilterRules);
            var data = await _context.ComStages.Where(filters)
                       .OrderBy($"{request.Sort} {request.Order}")
                       .ProjectTo<ComStageDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<ComStageDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["ComStages"]);
            return result;
        }
    }
}

