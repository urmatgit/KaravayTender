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
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Queries.Pagination
{
    public class AreasWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<AreaDto>>
    {
       
    }
    
    public class AreasWithPaginationQueryHandler :
         IRequestHandler<AreasWithPaginationQuery, PaginatedData<AreaDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AreasWithPaginationQueryHandler> _localizer;

        public AreasWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<AreasWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<AreaDto>> Handle(AreasWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AreasWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<Area>(request.FilterRules);
           var data = await _context.Areas.Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<AreaDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

