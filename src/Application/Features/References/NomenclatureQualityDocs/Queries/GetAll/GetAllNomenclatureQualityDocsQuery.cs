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
using CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.DTOs;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Queries.GetAll
{
    public class GetAllNomenclatureQualityDocsQuery : IRequest<IEnumerable<NomenclatureQualityDocDto>>
    {
       
    }
    
    public class GetAllNomenclatureQualityDocsQueryHandler :
         IRequestHandler<GetAllNomenclatureQualityDocsQuery, IEnumerable<NomenclatureQualityDocDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllNomenclatureQualityDocsQueryHandler> _localizer;

        public GetAllNomenclatureQualityDocsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllNomenclatureQualityDocsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<NomenclatureQualityDocDto>> Handle(GetAllNomenclatureQualityDocsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllNomenclatureQualityDocsQueryHandler method 
            var data = await _context.NomenclatureQualityDocs
                         .ProjectTo<NomenclatureQualityDocDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

