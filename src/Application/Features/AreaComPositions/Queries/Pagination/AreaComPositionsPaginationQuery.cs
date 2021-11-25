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
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.AreaComPositions.Queries.Pagination
{
    public class AreaComPositionsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<AreaComPositionDto>>
    {
       
    }
    
    public class AreaComPositionsWithPaginationQueryHandler :
         IRequestHandler<AreaComPositionsWithPaginationQuery, PaginatedData<AreaComPositionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AreaComPositionsWithPaginationQueryHandler> _localizer;

        public AreaComPositionsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<AreaComPositionsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<AreaComPositionDto>> Handle(AreaComPositionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AreaComPositionsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<AreaComPosition>(request.FilterRules);
           var data = await _context.AreaComPositions.Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<AreaComPositionDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

