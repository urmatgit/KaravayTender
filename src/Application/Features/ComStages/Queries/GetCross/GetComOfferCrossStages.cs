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
using CleanArchitecture.Razor.Application.Common.Specification;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Queries.GetCross
{

    public class GetComOfferCrossStages : IRequest<Result<ComStageCrossDTO>>
    {
        public int Stage { get; set; }
        public int ComOfferId { get; set; }
    }
    public class GetComOfferCrossStagesHandler :
        IRequestHandler<GetComOfferCrossStages, Result<ComStageCrossDTO>>
        
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetComOfferCrossStagesHandler> _localizer;
        public GetComOfferCrossStagesHandler(IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetComOfferCrossStagesHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }
        public async Task<Result<ComStageCrossDTO>> Handle(GetComOfferCrossStages request, CancellationToken cancellationToken)
        {
            ComStageCrossDTO result = new ComStageCrossDTO();
            var data = await _context.ComStages
                 .Specify(new FilterByStageQuerySpec(request.Stage, request.ComOfferId))
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
            if (dataDto.Any())
            {
                //Header
                var contragents = from s in dataDto
                                  from c in s.StageCompositions
                                  group s by c.Contragent into contrs
                                  select contrs.Key;
                var Participants = from c in dataDto.GroupBy(r => r.ComOffer).Select(g => g.Key).FirstOrDefault().ComParticipants
                                   select new
                                   {
                                       id = c.ContragentId,
                                       status = c.Status
                                   };
                var status = from s in contragents
                             join p in Participants on s.Id equals p.id into ps
                             from p in ps.DefaultIfEmpty()
                             select new ForTableHeader
                             {
                                 Contragent = s,
                                 ParticipantStatus = p == null ? "" : p.status.ToDescriptionString()
                             };
                if (status.Any()) {
                    result.Header = status.Select(c => $"{c.Contragent.Name}({c.ParticipantStatus}) ").ToArray();
                }
                //Body
            }
            return Result<ComStageCrossDTO>.Success(result) ;
        }
        public class FilterByStageQuerySpec : Specification<ComStage>
        {
            public FilterByStageQuerySpec(int stage, int comOfferId)
            {

                if (stage == 0)
                    Criteria = p => p.ComOfferId == comOfferId;
                else
                    Criteria = p => p.Number == stage && p.ComOfferId == comOfferId;
            }
            public FilterByStageQuerySpec(int comOfferId)
            {
                Criteria = p => p.ComOfferId == comOfferId;
            }

        }
    }
}
