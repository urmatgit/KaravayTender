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
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Queries.GetAll
{
    public class GetAllUnitOfsQuery : IRequest<IEnumerable<UnitOfDto>>
    {
       
    }
    
    public class GetAllUnitOfsQueryHandler :
         IRequestHandler<GetAllUnitOfsQuery, IEnumerable<UnitOfDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllUnitOfsQueryHandler> _localizer;

        public GetAllUnitOfsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllUnitOfsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<UnitOfDto>> Handle(GetAllUnitOfsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllUnitOfsQueryHandler method 
            var data = await _context.UnitOfs
                         .ProjectTo<UnitOfDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

