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
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.Queries.Export
{
    public class ExportQualityDocsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    public class DownloadQualityDocsQuery : IRequest<string>
    {
        public string name { get; set; }
        
    }

    public class ExportQualityDocsQueryHandler :
         IRequestHandler<ExportQualityDocsQuery, byte[]>,
        IRequestHandler<DownloadQualityDocsQuery, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportQualityDocsQueryHandler> _localizer;

        public ExportQualityDocsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportQualityDocsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportQualityDocsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportQualityDocsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<QualityDoc>(request.FilterRules);
            var data = await _context.QualityDocs.Where(filters)
                       .OrderBy($"{request.Sort} {request.Order}")
                       .ProjectTo<QualityDocDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<QualityDocDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["QualityDocs"]);
            return result;
        }

        public async Task<string> Handle(DownloadQualityDocsQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.QualityDocs.Where(x => x.Name == request.name).SingleOrDefaultAsync(cancellationToken);
            if (data != null)
                return data.URL;
            else
                return "";

        }
    }
}

