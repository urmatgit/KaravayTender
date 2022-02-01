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
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Logs.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Domain.Entities.Log;
using MediatR;

namespace CleanArchitecture.Razor.Application.Logs.Queries.PaginationQuery
{
    public class LogsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<LogDto>>
    {


    }
    public class LogsQueryHandler : IRequestHandler<LogsWithPaginationQuery, PaginatedData<LogDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LogsQueryHandler(
            ICurrentUserService currentUserService,
            IApplicationDbContext context,
            IMapper mapper
            )
        {
            _currentUserService = currentUserService;
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedData<LogDto>> Handle(LogsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var filters = PredicateBuilder.FromFilter<Logger>(request.FilterRules);

            var data = await _context.Loggers
                .Where(filters)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<LogDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);

            return data;
        }


    }
}
