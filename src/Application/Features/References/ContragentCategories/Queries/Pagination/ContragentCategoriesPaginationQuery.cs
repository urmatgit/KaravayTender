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
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Queries.Pagination
{
    public class ContragentCategoriesWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ContragentCategoryDto>>
    {
       
    }
    
    public class ContragentCategoriesWithPaginationQueryHandler :
         IRequestHandler<ContragentCategoriesWithPaginationQuery, PaginatedData<ContragentCategoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ContragentCategoriesWithPaginationQueryHandler> _localizer;

        public ContragentCategoriesWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ContragentCategoriesWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<ContragentCategoryDto>> Handle(ContragentCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ContragentCategoriesWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<ContragentCategory>(request.FilterRules);
           var data = await _context.ContragentCategories.Where(filters)
                .OrderBy("{request.Sort} {request.Order}")
                .ProjectTo<ContragentCategoryDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

