// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Directions.Queries.GetAll
{
    public class GetAllDirectionsQuery : IRequest<IEnumerable<DirectionDto>>
    {
        public bool HideService { get; set; } 
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

            IQueryable<Direction> data =  _context.Directions
                         .Include(d => d.Categories);
            if (request.HideService)
                data = data.Where(x => x.IsService == false);
            var dataRestult=await data.ProjectTo<DirectionDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return dataRestult;
        }
    }
}

