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
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Queries.GetAll
{
    public class GetAllComPositionsQuery : IRequest<IEnumerable<ComPositionDto>>
    {
       
    }
    
    public class GetAllComPositionsQueryHandler :
         IRequestHandler<GetAllComPositionsQuery, IEnumerable<ComPositionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllComPositionsQueryHandler> _localizer;

        public GetAllComPositionsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllComPositionsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ComPositionDto>> Handle(GetAllComPositionsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllComPositionsQueryHandler method 
            var data = await _context.ComPositions
                         .ProjectTo<ComPositionDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

