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
    public class GetByIdNomenclatureQuery : IRequest<NomenclatureDto>
    {
       public int Id { get; set; }
    }
    
    public class GetByIdNomenclatureQueryHandler :
         IRequestHandler<GetByIdNomenclatureQuery, NomenclatureDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetByIdNomenclatureQueryHandler> _localizer;

        public GetByIdNomenclatureQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetByIdNomenclatureQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<NomenclatureDto> Handle(GetByIdNomenclatureQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetByIdNomenclatureQueryHandler method 
            var data = await _context.Nomenclatures
                         .Include(c=>c.Category)
                         .Include(u=>u.UnitOf)
                         .Include(s=>s.NomenclatureQualityDocs)
                         .ThenInclude(q=>q.QualityDoc)
                         .Where(n=>n.Id==request.Id)
                         .ProjectTo<NomenclatureDto>(_mapper.ConfigurationProvider)
                         .FirstOrDefaultAsync(cancellationToken);
            return data;
        }
    }
}

