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
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Application.Common.Specification;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Queries.Pagination
{
    public class StageCompositionsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<StageCompositionDto>>
    {
        public int ComStageId { get; set; }
    }
    
    public class StageCompositionsWithPaginationQueryHandler :
         IRequestHandler<StageCompositionsWithPaginationQuery, PaginatedData<StageCompositionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<StageCompositionsWithPaginationQueryHandler> _localizer;

        public StageCompositionsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<StageCompositionsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<StageCompositionDto>> Handle(StageCompositionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing StageCompositionsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<StageComposition>(request.FilterRules);
           var data = await _context.StageCompositions
                .Specify(new FilterByComStageQuerySpec(request.ComStageId))
                .Where(filters)
                .Include(s=>s.Contragent)
                .Include(s=>s.ComPosition)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<StageCompositionDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
        public class FilterByComStageQuerySpec : Specification<StageComposition>
        {
            public FilterByComStageQuerySpec(int comStageId)
            {
                Criteria = p => p.ComStageId == comStageId;
            }


        }
    }
}

