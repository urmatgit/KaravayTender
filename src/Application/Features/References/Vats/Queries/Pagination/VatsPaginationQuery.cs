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
using CleanArchitecture.Razor.Application.Features.References.Vats.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Queries.Pagination
{
    public class VatsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<VatDto>>
    {

    }

    public class VatsWithPaginationQueryHandler :
         IRequestHandler<VatsWithPaginationQuery, PaginatedData<VatDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<VatsWithPaginationQueryHandler> _localizer;

        public VatsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<VatsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<VatDto>> Handle(VatsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing VatsWithPaginationQueryHandler method 
            var filters = PredicateBuilder.FromFilter<Vat>(request.FilterRules);
            var data = await _context.Vats.Where(filters)
                 .OrderBy($"{request.Sort} {request.Order}")
                 .ProjectTo<VatDto>(_mapper.ConfigurationProvider)
                 .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }
    }
}

