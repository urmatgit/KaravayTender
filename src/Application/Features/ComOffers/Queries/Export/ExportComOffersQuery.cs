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
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Queries.Export
{
    public class ExportComOffersQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportComOffersQueryHandler :
         IRequestHandler<ExportComOffersQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportComOffersQueryHandler> _localizer;

        public ExportComOffersQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportComOffersQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportComOffersQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportComOffersQueryHandler method 
            var filters = PredicateBuilder.FromFilter<ComOffer>(request.FilterRules);
            var data = await _context.ComOffers.Where(filters)
                       .OrderBy($"{request.Sort} {request.Order}")
                       .ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<ComOfferDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["ComOffers"]);
            return result;
        }
    }
}

