// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Queries.GetAll
{
    //ParticipantCrossDto
   public class GetParticipantsLastWithQuery  : IRequest<Result<IEnumerable<ParticipantCrossDto>>>
    {
        public int ComOfferId { get; set; }
    }
    public class GetParticipantsLastWithQueryHandler :
        IRequestHandler<GetParticipantsLastWithQuery, Result<IEnumerable<ParticipantCrossDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetParticipantsLastWithQueryHandler> _localizer;

        public GetParticipantsLastWithQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetParticipantsLastWithQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<IEnumerable<ParticipantCrossDto>>> Handle(GetParticipantsLastWithQuery request, CancellationToken cancellationToken)
        {

            var data =await  _context
                        .GetParicipantsForLastStage(request.ComOfferId)
                        .ToListAsync(cancellationToken);
            
            return Result<IEnumerable<ParticipantCrossDto>>.Success (data);
        }
    }
}
