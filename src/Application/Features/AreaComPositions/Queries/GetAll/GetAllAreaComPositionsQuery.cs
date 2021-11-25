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
using CleanArchitecture.Razor.Application.Features.AreaComPositions.DTOs;

namespace CleanArchitecture.Razor.Application.Features.AreaComPositions.Queries.GetAll
{
    public class GetAllAreaComPositionsQuery : IRequest<IEnumerable<AreaComPositionDto>>
    {
       
    }
    
    public class GetAllAreaComPositionsQueryHandler :
         IRequestHandler<GetAllAreaComPositionsQuery, IEnumerable<AreaComPositionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllAreaComPositionsQueryHandler> _localizer;

        public GetAllAreaComPositionsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllAreaComPositionsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<AreaComPositionDto>> Handle(GetAllAreaComPositionsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllAreaComPositionsQueryHandler method 
            var data = await _context.AreaComPositions
                         .ProjectTo<AreaComPositionDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

