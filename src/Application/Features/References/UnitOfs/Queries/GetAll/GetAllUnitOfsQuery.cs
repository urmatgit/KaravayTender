// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

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

