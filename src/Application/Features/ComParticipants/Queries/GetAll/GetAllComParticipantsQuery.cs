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

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Queries.GetAll
{
    public class GetAllComParticipantsQuery : IRequest<IEnumerable<ComParticipantDto>>
    {
       
    }
    
    public class GetAllComParticipantsQueryHandler :
         IRequestHandler<GetAllComParticipantsQuery, IEnumerable<ComParticipantDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllComParticipantsQueryHandler> _localizer;

        public GetAllComParticipantsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllComParticipantsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ComParticipantDto>> Handle(GetAllComParticipantsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllComParticipantsQueryHandler method 
            var data = await _context.ComParticipants
                         .ProjectTo<ComParticipantDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }
}

