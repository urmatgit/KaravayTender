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
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Queries.Pagination
{
    public class ComPositionsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ComPositionDto>>
    {
       
    }
    
    public class ComPositionsWithPaginationQueryHandler :
         IRequestHandler<ComPositionsWithPaginationQuery, PaginatedData<ComPositionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ComPositionsWithPaginationQueryHandler> _localizer;

        public ComPositionsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ComPositionsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<ComPositionDto>> Handle(ComPositionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ComPositionsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<ComPosition>(request.FilterRules);
           var data = await _context.ComPositions.Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<ComPositionDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

