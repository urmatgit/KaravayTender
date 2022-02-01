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
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.DTOs;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.Queries.GetAll
{
    public class GetAllQualityDocsQuery : IRequest<IEnumerable<QualityDocDto>>
    {
       
    }
    
    public class GetAllQualityDocsQueryHandler :
         IRequestHandler<GetAllQualityDocsQuery, IEnumerable<QualityDocDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllQualityDocsQueryHandler> _localizer;

        public GetAllQualityDocsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllQualityDocsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<QualityDocDto>> Handle(GetAllQualityDocsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllQualityDocsQueryHandler method 
            var data = await _context.QualityDocs
                         .ProjectTo<QualityDocDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

