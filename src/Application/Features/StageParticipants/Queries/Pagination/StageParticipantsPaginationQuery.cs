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
using CleanArchitecture.Razor.Application.Features.StageParticipants.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Queries.Pagination
{
    public class StageParticipantsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<StageParticipantDto>>
    {
       
    }
    
    public class StageParticipantsWithPaginationQueryHandler :
         IRequestHandler<StageParticipantsWithPaginationQuery, PaginatedData<StageParticipantDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<StageParticipantsWithPaginationQueryHandler> _localizer;

        public StageParticipantsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<StageParticipantsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<StageParticipantDto>> Handle(StageParticipantsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing StageParticipantsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<StageParticipant>(request.FilterRules);
           var data = await _context.StageParticipants.Where(filters)
                .OrderBy("{request.Sort} {request.Order}")
                .ProjectTo<StageParticipantDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

