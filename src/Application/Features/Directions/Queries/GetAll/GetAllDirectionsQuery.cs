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

namespace CleanArchitecture.Razor.Application.Features.Directions.Queries.GetAll
{
    public class GetAllDirectionsQuery : IRequest<IEnumerable<DirectionDto>>
    {
       
    }
    
    public class GetAllDirectionsQueryHandler :
         IRequestHandler<GetAllDirectionsQuery, IEnumerable<DirectionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllDirectionsQueryHandler> _localizer;

        public GetAllDirectionsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllDirectionsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<DirectionDto>> Handle(GetAllDirectionsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllDirectionsQueryHandler method 
            var data = await _context.Directions
                         .ProjectTo<DirectionDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

