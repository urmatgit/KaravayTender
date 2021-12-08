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
using CleanArchitecture.Razor.Application.Common.Specification;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Queries.GetBy
{
   public class GetByStageQuery : IRequest<ComStageDto>
    {
        public int Stage { get; set; }
        public int ComOfferId { get; set; }
    }
    public class GetByIdComOfferIdQuery : IRequest<IEnumerable<ComStageDto>>
    {
     public int ComOfferId { get; set; }   
    }
    public class GetByIdComStageQueryHandler :
        IRequestHandler<GetByStageQuery, ComStageDto>,
        IRequestHandler<GetByIdComOfferIdQuery, IEnumerable<ComStageDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetByIdComStageQueryHandler> _localizer;
        public GetByIdComStageQueryHandler(
             IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetByIdComStageQueryHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }
       public async Task<ComStageDto>  Handle(GetByStageQuery request, CancellationToken cancellationToken)
        {
              var data = await _context.ComStages
                 .Specify(new FilterByStageQuerySpec(request.Stage,request.ComOfferId))
                 .Include(s => s.StageCompositions)
                .ThenInclude(c => c.Contragent)
                .Include(s => s.StageCompositions)
                .ThenInclude(c => c.ComPosition)
                .ThenInclude(c=>c.Nomenclature)
                .Include(s => s.ComOffer)
                .ThenInclude(p=>p.ComParticipants)
                .ThenInclude(p => p.Contragent)
                .FirstOrDefaultAsync(cancellationToken);

            var dataDto = _mapper.Map<ComStageDto>(data);
            return dataDto;
        }

        public async Task<IEnumerable<ComStageDto>> Handle(GetByIdComOfferIdQuery request, CancellationToken cancellationToken)
        {
            var data =await _context.ComStages
                 .Specify(new FilterByComOfferQuerySpec(request.ComOfferId))
                 .Include(s => s.StageCompositions)
                .ThenInclude(c => c.Contragent)
                .Include(s => s.StageCompositions)
                .ThenInclude(c => c.ComPosition)
                .ThenInclude(c => c.Nomenclature)
                .Include(s => s.ComOffer)
                .ThenInclude(p => p.ComParticipants)
                .ThenInclude(p => p.Contragent)
                .ToListAsync(cancellationToken);

            var dataDto = _mapper.Map<IEnumerable<ComStageDto>>(data);
            return dataDto;
        }

        public class FilterByComOfferQuerySpec : Specification<ComStage>
        {
            public FilterByComOfferQuerySpec(int comOfferId)
            {
                Criteria = p => p.ComOfferId == comOfferId;
            }


        }
        public class FilterByStageQuerySpec : Specification<ComStage>
        {
            public FilterByStageQuerySpec(int stage, int comOfferId)
            {
                Criteria = p =>  p.Number == stage && p.ComOfferId==comOfferId;
            }
            public FilterByStageQuerySpec( int comOfferId)
            {
                Criteria = p => p.ComOfferId == comOfferId;
            }

        }
    }
}
