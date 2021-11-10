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
using CleanArchitecture.Razor.Application.Features.References.Vats.DTOs;

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

