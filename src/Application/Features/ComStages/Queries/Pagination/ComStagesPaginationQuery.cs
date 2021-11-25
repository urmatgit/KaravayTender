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
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Queries.Pagination
{
    public class ComStagesWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ComStageDto>>
    {
       
    }
    
    public class ComStagesWithPaginationQueryHandler :
         IRequestHandler<ComStagesWithPaginationQuery, PaginatedData<ComStageDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ComStagesWithPaginationQueryHandler> _localizer;

        public ComStagesWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ComStagesWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<ComStageDto>> Handle(ComStagesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ComStagesWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<ComStage>(request.FilterRules);
           var data = await _context.ComStages.Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<ComStageDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

