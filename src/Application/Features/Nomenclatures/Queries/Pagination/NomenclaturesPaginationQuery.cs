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
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Application.Common.Models;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Queries.Pagination
{
    public class NomenclaturesWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<NomenclatureDto>>
    {
       
    }
    
    public class NomenclaturesWithPaginationQueryHandler :
         IRequestHandler<NomenclaturesWithPaginationQuery, PaginatedData<NomenclatureDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<NomenclaturesWithPaginationQueryHandler> _localizer;

        public NomenclaturesWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<NomenclaturesWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<NomenclatureDto>> Handle(NomenclaturesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing NomenclaturesWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<Nomenclature>(request.FilterRules);
           var data = await _context.Nomenclatures.Where(filters)
                .Include(d=>d.Direction)
                .Include(c=>c.Category)
                .Include(u=>u.UnitOf)
                .Include(v=>v.Vat)
                .Include(q=>q.NomenclatureQualityDocs)
                .ThenInclude(qn=>qn.Nomenclature)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<NomenclatureDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

