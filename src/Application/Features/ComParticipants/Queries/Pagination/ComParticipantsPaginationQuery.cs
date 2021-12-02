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
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Application.Common.Specification;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Queries.Pagination
{
    public class ComParticipantsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ComParticipantDto>>
    {
        public int ComOfferId { get; set; }
    }
    
    public class ComParticipantsWithPaginationQueryHandler :
         IRequestHandler<ComParticipantsWithPaginationQuery, PaginatedData<ComParticipantDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ComParticipantsWithPaginationQueryHandler> _localizer;

        public ComParticipantsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ComParticipantsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<ComParticipantDto>> Handle(ComParticipantsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ComParticipantsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<ComParticipant>(request.FilterRules);
           var data = await _context.ComParticipants
                .Specify(new FilterByComOfferQuerySpec(request.ComOfferId))
                .Where(filters)
                .Include(c=>c.Contragent)
                .OrderBy($"{request.Sort} {request.Order}")
                .PaginatedDataAsync(request.Page, request.Rows);
            var dataDto = _mapper.Map<IEnumerable<ComParticipantDto>>(data.rows);
            //.ProjectTo<ComParticipantDto>(_mapper.ConfigurationProvider)
            return new PaginatedData<ComParticipantDto>(dataDto,data.total);
        }
        public class FilterByComOfferQuerySpec : Specification<ComParticipant>
        {
            public FilterByComOfferQuerySpec(int comOfferId)
            {
                Criteria = p => p.ComOfferId == comOfferId;
            }


        }
    }
}
