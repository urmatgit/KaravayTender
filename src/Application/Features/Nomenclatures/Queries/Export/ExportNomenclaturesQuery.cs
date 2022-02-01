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
using CleanArchitecture.Razor.Application.Features.Nomenclatures.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Queries.Export
{
    public class ExportNomenclaturesQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportNomenclaturesQueryHandler :
         IRequestHandler<ExportNomenclaturesQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportNomenclaturesQueryHandler> _localizer;

        public ExportNomenclaturesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportNomenclaturesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportNomenclaturesQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportNomenclaturesQueryHandler method 
            var filters = PredicateBuilder.FromFilter<Nomenclature>(request.FilterRules);
            var data = await _context.Nomenclatures.Where(filters)
                       .OrderBy($"{request.Sort} {request.Order}")
                       .ProjectTo<NomenclatureDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<NomenclatureDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["Nomenclatures"]);
            return result;
        }
    }
}

