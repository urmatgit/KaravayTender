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

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Queries.GetAll
{
    public class GetAllComOffersQuery : IRequest<IEnumerable<ComOfferDto>>
    {
       
    }
    
    public class GetAllComOffersQueryHandler :
         IRequestHandler<GetAllComOffersQuery, IEnumerable<ComOfferDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllComOffersQueryHandler> _localizer;

        public GetAllComOffersQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllComOffersQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ComOfferDto>> Handle(GetAllComOffersQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllComOffersQueryHandler method 
            var data = await _context.ComOffers
                         .ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

