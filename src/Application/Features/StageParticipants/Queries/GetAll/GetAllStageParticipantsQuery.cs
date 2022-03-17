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

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Queries.GetAll
{
    public class GetAllStageParticipantsQuery : IRequest<IEnumerable<StageParticipantDto>>
    {
       
    }
    
    public class GetAllStageParticipantsQueryHandler :
         IRequestHandler<GetAllStageParticipantsQuery, IEnumerable<StageParticipantDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllStageParticipantsQueryHandler> _localizer;

        public GetAllStageParticipantsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllStageParticipantsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<StageParticipantDto>> Handle(GetAllStageParticipantsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllStageParticipantsQueryHandler method 
            var data = await _context.StageParticipants
                         .ProjectTo<StageParticipantDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

