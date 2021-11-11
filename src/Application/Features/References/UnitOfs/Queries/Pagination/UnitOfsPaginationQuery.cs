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
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Queries.Pagination
{
    public class UnitOfsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<UnitOfDto>>
    {

    }

    public class UnitOfsWithPaginationQueryHandler :
         IRequestHandler<UnitOfsWithPaginationQuery, PaginatedData<UnitOfDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UnitOfsWithPaginationQueryHandler> _localizer;

        public UnitOfsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<UnitOfsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<UnitOfDto>> Handle(UnitOfsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UnitOfsWithPaginationQueryHandler method 
            var filters = PredicateBuilder.FromFilter<UnitOf>(request.FilterRules);
            var data = await _context.UnitOfs.Where(filters)
                 .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<UnitOfDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

