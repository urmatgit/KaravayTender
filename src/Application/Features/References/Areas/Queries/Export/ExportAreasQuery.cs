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
using CleanArchitecture.Razor.Application.Features.References.Areas.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Queries.Export
{
    public class ExportAreasQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportAreasQueryHandler :
         IRequestHandler<ExportAreasQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportAreasQueryHandler> _localizer;

        public ExportAreasQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportAreasQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportAreasQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportAreasQueryHandler method 
            var filters = PredicateBuilder.FromFilter<Area>(request.FilterRules);
            var data = await _context.Areas.Where(filters)
                       .OrderBy($"{request.Sort} {request.Order}")
                       .ProjectTo<AreaDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<AreaDto, object>>()
                {
                    { _localizer["Id"], item => item.Id },
                    { _localizer["Name"], item => item.Name },
                    { _localizer["Address"], item => item.Address },
                    { _localizer["Description"], item => item.Description },
                }
                , _localizer["Areas"]);
            return result;
        }
    }
}

