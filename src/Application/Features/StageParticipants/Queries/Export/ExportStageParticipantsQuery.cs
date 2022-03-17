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
using CleanArchitecture.Razor.Domain.Entities.Karavay;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Queries.Export
{
    public class ExportStageParticipantsQuery : IRequest<byte[]>
    {
        public string FilterRules { get; set; }
        public string Sort { get; set; } = "Id";
        public string Order { get; set; } = "desc";
    }
    
    public class ExportStageParticipantsQueryHandler :
         IRequestHandler<ExportStageParticipantsQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportStageParticipantsQueryHandler> _localizer;

        public ExportStageParticipantsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportStageParticipantsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<byte[]> Handle(ExportStageParticipantsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ExportStageParticipantsQueryHandler method 
            var filters = PredicateBuilder.FromFilter<StageParticipant>(request.FilterRules);
            var data = await _context.StageParticipants.Where(filters)
                       .OrderBy("{request.Sort} {request.Order}")
                       .ProjectTo<StageParticipantDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<StageParticipantDto, object>>()
                {
                    //{ _localizer["Id"], item => item.Id },
                }
                , _localizer["StageParticipants"]);
            return result;
        }
    }
}

