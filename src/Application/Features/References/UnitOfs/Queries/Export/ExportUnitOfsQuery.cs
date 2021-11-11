// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Queries.Export
{
    public class ExportUnitOfsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }

    public class ExportUnitOfsQueryHandler :
         IRequestHandler<ExportUnitOfsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportUnitOfsQueryHandler> _localizer;

        public ExportUnitOfsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportUnitOfsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportUnitOfsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportUnitOfsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<UnitOf>(request.FilterRules);
            var data = await _context.UnitOfs.Where(filters)
                       .OrderBy("{request.Sort} {request.Order}")
                       .ProjectTo<UnitOfDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<UnitOfDto, object>>()
                {
                    { _localizer["Id"], item => item.Id },
                    { _localizer["Name"], item => item.Name },
                }
                , _localizer["UnitOfs"]);
            return result;
        }
    }
}

