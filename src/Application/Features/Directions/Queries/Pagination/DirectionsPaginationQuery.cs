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
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;

namespace CleanArchitecture.Razor.Application.Features.Directions.Queries.Pagination
{
    public class DirectionsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<DirectionDto>>
    {
       
    }
    
    public class DirectionsWithPaginationQueryHandler :
         IRequestHandler<DirectionsWithPaginationQuery, PaginatedData<DirectionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DirectionsWithPaginationQueryHandler> _localizer;

        public DirectionsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<DirectionsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<DirectionDto>> Handle(DirectionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DirectionsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<Direction>(request.FilterRules);
           var data = await _context.Directions.Where(filters)
                .OrderBy("{request.Sort} {request.Order}")
                .ProjectTo<DirectionDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

