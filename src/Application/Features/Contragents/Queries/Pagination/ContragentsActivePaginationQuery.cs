// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Common.Specification;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Queries.Pagination
{
    public class ContragentsActivePaginationQuery : PaginationRequest, IRequest<PaginatedData<ContragentDto>>
    {
        public int ComOfferId { get; set; }
    }

    public class ContragentsActivePaginationQueryHandler :
         IRequestHandler<ContragentsActivePaginationQuery, PaginatedData<ContragentDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ContragentsActivePaginationQueryHandler> _localizer;
        private readonly IIdentityService _identityService;
        public ContragentsActivePaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ContragentsActivePaginationQueryHandler> localizer,
            IIdentityService identityService
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _identityService = identityService;
        }

        public async Task<PaginatedData<ContragentDto>> Handle(ContragentsActivePaginationQuery request, CancellationToken cancellationToken)
        {

            
            var filters = PredicateBuilder.FromFilter<Contragent>(request.FilterRules);
            var ExistContrs = _context.ComOffers
                            .Include(x=>x.ComParticipants)
                            .Where(x => x.Id == request.ComOfferId)
                            .SingleOrDefault();

            var data = await _context.Contragents
                .Where(filters)
                .Where(x=>! _context.ComParticipants.Where(p=>p.ComOfferId==request.ComOfferId).Select(z=>z.ContragentId).Contains(x.Id))
                .Where(x=>_context.ComOffers.Where(c=>c.Id==request.ComOfferId).SingleOrDefault().DirectionId==x.DirectionId)
                .Specify(new ContragentActiveQuerySpec())
                .Include(i => i.Direction)
                .Include(u=>u.Manager)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<ContragentDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);

            
            return data;
        }
        public class ContragentActiveQuerySpec : Specification<Contragent>
        {
            public ContragentActiveQuerySpec()
            {
                Criteria = p => p.Status == Domain.Enums.ContragentStatus.Registered;
            }


        }
    }
}

