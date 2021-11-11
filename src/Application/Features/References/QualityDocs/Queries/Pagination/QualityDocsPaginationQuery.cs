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
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.Queries.Pagination
{
    public class QualityDocsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<QualityDocDto>>
    {
       
    }
    
    public class QualityDocsWithPaginationQueryHandler :
         IRequestHandler<QualityDocsWithPaginationQuery, PaginatedData<QualityDocDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<QualityDocsWithPaginationQueryHandler> _localizer;

        public QualityDocsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<QualityDocsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<QualityDocDto>> Handle(QualityDocsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing QualityDocsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<QualityDoc>(request.FilterRules);
           var data = await _context.QualityDocs.Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<QualityDocDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

