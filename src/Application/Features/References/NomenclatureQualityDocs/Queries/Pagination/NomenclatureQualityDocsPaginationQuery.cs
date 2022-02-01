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
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Queries.Pagination
{
    public class NomenclatureQualityDocsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<NomenclatureQualityDocDto>>
    {
       
    }
    
    public class NomenclatureQualityDocsWithPaginationQueryHandler :
         IRequestHandler<NomenclatureQualityDocsWithPaginationQuery, PaginatedData<NomenclatureQualityDocDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<NomenclatureQualityDocsWithPaginationQueryHandler> _localizer;

        public NomenclatureQualityDocsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<NomenclatureQualityDocsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<NomenclatureQualityDocDto>> Handle(NomenclatureQualityDocsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing NomenclatureQualityDocsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<NomenclatureQualityDoc>(request.FilterRules);
           var data = await _context.NomenclatureQualityDocs.Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<NomenclatureQualityDocDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

