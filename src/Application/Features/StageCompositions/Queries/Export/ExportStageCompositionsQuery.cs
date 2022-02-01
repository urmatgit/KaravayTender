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
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Queries.Export
{
    public class ExportStageCompositionsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportStageCompositionsQueryHandler :
         IRequestHandler<ExportStageCompositionsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportStageCompositionsQueryHandler> _localizer;

        public ExportStageCompositionsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportStageCompositionsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportStageCompositionsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportStageCompositionsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<StageComposition>(request.FilterRules);
            var data = await _context.StageCompositions.Where(filters)
                       .OrderBy($"{request.Sort} {request.Order}")
                       .ProjectTo<StageCompositionDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<StageCompositionDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["StageCompositions"]);
            return result;
        }
    }
}

