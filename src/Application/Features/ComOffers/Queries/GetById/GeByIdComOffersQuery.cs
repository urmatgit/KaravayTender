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
using CleanArchitecture.Razor.Application.Common.Models;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Queries.GetAll
{
    public class GeByIdComOffersQuery : IRequest<Result<ComOfferDto>>
    {
       public int Id { get; set; }
        public bool FullInfo { get; set; } = true;
    }
    
    public class GeByIdComOffersQueryHandler :
         IRequestHandler<GeByIdComOffersQuery, Result<ComOfferDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GeByIdComOffersQueryHandler> _localizer;

        public GeByIdComOffersQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GeByIdComOffersQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<ComOfferDto>> Handle(GeByIdComOffersQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GeByIdComOffersQueryHandler method
            if (request.FullInfo)
            {
                var comoffer = await _context.ComOffers
                      .Include(c => c.ComPositions)
                      .Include(c => c.ComParticipants)
                      .Where(c => c.Id == request.Id)
                      .FirstOrDefaultAsync(cancellationToken);
                var dto = _mapper.Map<ComOfferDto>(comoffer);
                return Result<ComOfferDto>.Success(dto);
            }
            else
            {
                var comoffer = await _context.ComOffers
                      .Where(c => c.Id == request.Id)
                      .FirstOrDefaultAsync(cancellationToken);
                var dto = _mapper.Map<ComOfferDto>(comoffer);
                return Result<ComOfferDto>.Success(dto);
            }
        }
    }
}

