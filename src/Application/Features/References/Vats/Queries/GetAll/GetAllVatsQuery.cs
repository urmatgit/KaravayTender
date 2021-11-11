// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.References.Vats.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Queries.GetAll
{
    public class GetAllVatsQuery : IRequest<IEnumerable<VatDto>>
    {

    }

    public class GetAllVatsQueryHandler :
         IRequestHandler<GetAllVatsQuery, IEnumerable<VatDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllVatsQueryHandler> _localizer;

        public GetAllVatsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllVatsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<VatDto>> Handle(GetAllVatsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllVatsQueryHandler method 
            var data = await _context.Vats
                         .ProjectTo<VatDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

