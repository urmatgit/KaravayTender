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
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Queries.Pagination
{
    public class ComOffersWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ComOfferDto>>
    {
       
    }
    
    public class ComOffersWithPaginationQueryHandler :
         IRequestHandler<ComOffersWithPaginationQuery, PaginatedData<ComOfferDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ComOffersWithPaginationQueryHandler> _localizer;

        public ComOffersWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ComOffersWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<ComOfferDto>> Handle(ComOffersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ComOffersWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<ComOffer>(request.FilterRules);
           var data = await _context.ComOffers.Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

