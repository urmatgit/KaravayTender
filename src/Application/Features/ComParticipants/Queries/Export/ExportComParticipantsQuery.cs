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
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Queries.Export
{
    public class ExportComParticipantsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportComParticipantsQueryHandler :
         IRequestHandler<ExportComParticipantsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportComParticipantsQueryHandler> _localizer;

        public ExportComParticipantsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportComParticipantsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportComParticipantsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportComParticipantsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<ComParticipant>(request.FilterRules);
            var data = await _context.ComParticipants.Where(filters)
                       .OrderBy($"{request.Sort} {request.Order}")
                       .ProjectTo<ComParticipantDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<ComParticipantDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["ComParticipants"]);
            return result;
        }
    }
}

