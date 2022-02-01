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
using CleanArchitecture.Razor.Application.Features.References.Areas.DTOs;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Queries.GetAll
{
    public class GetAllAreasQuery : IRequest<IEnumerable<AreaDto>>
    {
       
    }
    
    public class GetAllAreasQueryHandler :
         IRequestHandler<GetAllAreasQuery, IEnumerable<AreaDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllAreasQueryHandler> _localizer;

        public GetAllAreasQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllAreasQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<AreaDto>> Handle(GetAllAreasQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllAreasQueryHandler method 
            var data = await _context.Areas
                         .ProjectTo<AreaDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

