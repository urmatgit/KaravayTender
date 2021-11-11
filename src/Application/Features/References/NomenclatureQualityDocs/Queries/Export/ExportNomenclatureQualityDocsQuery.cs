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
using CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.DTOs;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Queries.Export
{
    public class ExportNomenclatureQualityDocsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportNomenclatureQualityDocsQueryHandler :
         IRequestHandler<ExportNomenclatureQualityDocsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportNomenclatureQualityDocsQueryHandler> _localizer;

        public ExportNomenclatureQualityDocsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportNomenclatureQualityDocsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportNomenclatureQualityDocsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportNomenclatureQualityDocsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<NomenclatureQualityDoc>(request.FilterRules);
            var data = await _context.NomenclatureQualityDocs.Where(filters)
                       .OrderBy($"{request.Sort} {request.Order}")
                       .ProjectTo<NomenclatureQualityDocDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<NomenclatureQualityDocDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["NomenclatureQualityDocs"]);
            return result;
        }
    }
}

