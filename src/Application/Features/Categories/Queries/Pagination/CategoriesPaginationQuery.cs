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
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;

namespace CleanArchitecture.Razor.Application.Features.Categories.Queries.Pagination
{
    public class CategoriesWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<CategoryDto>>
    {
     public int DirectionId { get; set; }  
    }
    
    public class CategoriesWithPaginationQueryHandler :
         IRequestHandler<CategoriesWithPaginationQuery, PaginatedData<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CategoriesWithPaginationQueryHandler> _localizer;

        public CategoriesWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<CategoriesWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<CategoryDto>> Handle(CategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing CategoriesWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<Category>(request.FilterRules);
            if (request.DirectionId > 0)
            {
                
                    filters = filters.And(p => p.DirectionId == request.DirectionId);
                
            }
           var data = await _context.Categories.Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

