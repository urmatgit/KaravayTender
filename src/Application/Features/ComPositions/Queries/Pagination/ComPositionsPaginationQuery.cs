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
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Queries.Pagination
{
    public class ComPositionsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ComPositionDto>>
    {
       
    }
    
    public class ComPositionsWithPaginationQueryHandler :
         IRequestHandler<ComPositionsWithPaginationQuery, PaginatedData<ComPositionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ComPositionsWithPaginationQueryHandler> _localizer;

        public ComPositionsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ComPositionsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<ComPositionDto>> Handle(ComPositionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ComPositionsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<ComPosition>(request.FilterRules);
            try
            {
                var data = await _context.ComPositions.Where(filters)
                     .Include(n => n.Nomenclature)
                     .ThenInclude(n => n.Category)
                     .Include(a=>a.AreaComPositions)
                     .ThenInclude(a=>a.Area)
                    .Include(n => n.Nomenclature)
                    .ThenInclude(n => n.UnitOf)
                    .Include(n => n.Nomenclature)
                    .ThenInclude(n => n.Vat)
                    .OrderBy($"{request.Sort} {request.Order}")
                  
                    .PaginatedDataAsync(request.Page, request.Rows);
                  //.ProjectTo<ComPositionDto>(_mapper.ConfigurationProvider)
                  var datDto = _mapper.Map<IEnumerable<ComPositionDto>>(data.rows);
                return new PaginatedData<ComPositionDto>(datDto, data.total); ;
            }catch(Exception er)
            {

                return null;
            }
        }
    }
}

