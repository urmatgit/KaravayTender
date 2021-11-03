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
using CleanArchitecture.Razor.Application.Features.StatusLogs.DTOs;

namespace CleanArchitecture.Razor.Application.Features.StatusLogs.Queries.Export
{
    public class ExportStatusLogsQuery : IRequest<byte[]>
    {
        public string filterRules { get; set; }
        public string sort { get; set; } = "Id";
        public string order { get; set; } = "desc";
    }
    
    public class ExportStatusLogsQueryHandler :
         IRequestHandler<ExportStatusLogsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ExportStatusLogsQueryHandler> _localizer;
        private readonly IExcelService _excelService;

        public ExportStatusLogsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportStatusLogsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportStatusLogsQuery request, CancellationToken cancellationToken)
        {
            
            var filters = PredicateBuilder.FromFilter<Customer>(request.filterRules);
            var data = await _context.StatusLogs.Where(filters)
                .OrderBy($"{request.sort} {request.order}")
                .ProjectTo<StatusLogDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<StatusLogDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                    { _localizer["Id"], item => item.Id },
                    { _localizer["Date"], item => item.DateTime },
                    { _localizer["Status"], item => item.Status.ToDescriptionString() },
                    { _localizer["Manager"], item => "" }
                    

                }, _localizer["StatusLog"]
                );
            return result;
        }
    }
}

