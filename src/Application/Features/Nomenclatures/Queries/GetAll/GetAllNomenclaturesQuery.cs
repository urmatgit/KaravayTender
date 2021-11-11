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
using CleanArchitecture.Razor.Application.Features.Nomenclatures.DTOs;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Queries.GetAll
{
    public class GetAllNomenclaturesQuery : IRequest<IEnumerable<NomenclatureDto>>
    {
       
    }
    
    public class GetAllNomenclaturesQueryHandler :
         IRequestHandler<GetAllNomenclaturesQuery, IEnumerable<NomenclatureDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllNomenclaturesQueryHandler> _localizer;

        public GetAllNomenclaturesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllNomenclaturesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<NomenclatureDto>> Handle(GetAllNomenclaturesQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllNomenclaturesQueryHandler method 
            var data = await _context.Nomenclatures
                         .ProjectTo<NomenclatureDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

