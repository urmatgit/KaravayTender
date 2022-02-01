// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
using CleanArchitecture.Razor.Application.Features.StatusLogs.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StatusLogs.Queries.Pagination
{
    public class StatusLogsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<StatusLogDto>>
    {
        public int? ContragentId { get; set; }
    }

    public class StatusLogsWithPaginationQueryHandler :
         IRequestHandler<StatusLogsWithPaginationQuery, PaginatedData<StatusLogDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<StatusLogsWithPaginationQueryHandler> _localizer;
        private readonly IIdentityService _identityService;
        public StatusLogsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<StatusLogsWithPaginationQueryHandler> localizer,
            IIdentityService identityService
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _identityService = identityService;
        }

        public async Task<PaginatedData<StatusLogDto>> Handle(StatusLogsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var filters = PredicateBuilder.FromFilter<StatusLog>(request.FilterRules);
            if (request.ContragentId > 0)
            {
                filters = filters.And(p => p.ContragentId == request.ContragentId);
            }
            var managers = await _identityService.FetchUsersEx("");
            var data = await _context.StatusLogs.Where(filters)
                .Include(c => c.Contragent)
                .OrderBy($"{request.Sort} {request.Order}")

                .ProjectTo<StatusLogDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);

            foreach (var d in data.rows)
            {
                if (!string.IsNullOrEmpty(d.ManagerId))
                {
                    var manager = managers.FirstOrDefault(m => m.Id == d.ManagerId);

                    d.UserName = manager?.DisplayName ?? manager?.UserName;

                }
            }
            return data;
        }

    }
}

