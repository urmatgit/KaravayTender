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
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Queries.Pagination
{
    public class ContragentsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ContragentDto>>
    {
       
    }
    
    public class ContragentsWithPaginationQueryHandler :
         IRequestHandler<ContragentsWithPaginationQuery, PaginatedData<ContragentDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ContragentsWithPaginationQueryHandler> _localizer;

        public ContragentsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ContragentsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<ContragentDto>> Handle(ContragentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ContragentsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<Contragent>(request.FilterRules);
           var data = await _context.Contragents.Where(filters)
                .Include(i=>i.Direction)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<ContragentDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

