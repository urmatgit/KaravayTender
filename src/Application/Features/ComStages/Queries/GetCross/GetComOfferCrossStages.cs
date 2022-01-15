// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
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
using CleanArchitecture.Razor.Application.Features.ComStages.Queries.GetBy;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Queries.GetCross
{

    public class GetComOfferCrossStages : IRequest<Result<ComStageCrossDTO>>
    {
        /// <summary>
        /// 1 get last, 0 get all
        /// </summary>
        public int Stage { get; set; }
        public int ComOfferId { get; set; }
    }
    public class GetComOfferCrossStagesHandler :
        IRequestHandler<GetComOfferCrossStages, Result<ComStageCrossDTO>>
        
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetComOfferCrossStagesHandler> _localizer;
        private readonly IMediator _mediator;
        public GetComOfferCrossStagesHandler(IApplicationDbContext context,
            IMapper mapper,
            IMediator mediator,
            IStringLocalizer<GetComOfferCrossStagesHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _mediator = mediator;
        }
        public async Task<Result<ComStageCrossDTO>> Handle(GetComOfferCrossStages request, CancellationToken cancellationToken)
        {

            ComStageCrossDTO result = new ComStageCrossDTO();
            List<ComStageDto> queryResult = new List<ComStageDto>();
            IEnumerable<ForTableHeader> forTableHeaders= new List<ForTableHeader>() ;
            var dataLast = await _mediator.Send(new GetByStageLastDtoQuery() { ComOfferId = request.ComOfferId }, cancellationToken);
            if (request.Stage == 1) //Последний этап
            {
                
                if (dataLast != null)
                {
                    queryResult.Add(dataLast);
                    result.CurrentStage = dataLast.Number;
                    result.CurrentStageId = dataLast.Id;
                    result.DeadlineDate = dataLast.DeadlineDate;
                }
            }
            else
            {
                queryResult = (List<ComStageDto>) await _mediator.Send(new GetByComOfferIdQuery() { Stage = 0, ComOfferId = request.ComOfferId },cancellationToken);
                //queryResult = (List<ComStageDto>)data;



            }
            if (queryResult.Count > 0)
            {
                forTableHeaders = GetHeaders(queryResult);
                if (forTableHeaders.Any())
                {
                    result.Header = forTableHeaders.Select(c => $"{c.Contragent.Name} ({c.ParticipantStatus}) ").ToArray();
                }
                result.Body = GetBody(queryResult,request.Stage,dataLast);
            }
            
          
            
            
            return Result<ComStageCrossDTO>.Success(result) ;
        }

        private List<ExpandoObject> GetBody(IEnumerable<ComStageDto> dataDto, int stage,ComStageDto lastStage)
        {
         
            List<ExpandoObject> resultData = new List<ExpandoObject>();

            var postions = dataDto.SelectMany(
                                        c => c.StageCompositions.Select(p => new { position =  p.ComPosition, child = c, stage = p.ComStage })
                                    ).GroupBy(cs =>  new { cs.position, cs.stage.Number,cs.stage.Id }  )
                                    .Select(g => new
                                    {
                                        keys = g.Key,
                                        stagecoms = g.Select(cp => cp.child).OrderBy(o=>o.Number).ToList(),
                                        stage1=g.FirstOrDefault(s=>s.stage.Number==g.Key.Number)
                                        
                                    }).OrderBy(o =>o.keys.Number).ThenBy(o=> o.keys.position.Nomenclature.Name);

            
            foreach(var pos in postions)
            {
                
                
                
                    var row = new ExpandoObject() as IDictionary<string, object>;
                    row.Add($"NomName", pos.keys.position.Nomenclature.Name);
                
                    row.Add($"Stage",pos.keys. Number);
                row.Add($"StageId", pos.keys.Id);
                
                row.Add($"StageDeadline", pos.stage1.stage.DeadlineDate);
                    int Indexcontrgent = 0;
                decimal? GoodPrice = null; 
                foreach (var stagecom in pos.stage1.stage.StageCompositions.Where(s=>s.ComPosition.Id==pos.keys.position.Id).OrderBy(o=>o.Contragent.Name))
                    {
                    var status = GetStatus(pos.stage1.stage.StageParticipants, stagecom.ContragentId);
                   // if (status == ParticipantStatus.FailureParitipate && stage==1) continue;
                        Indexcontrgent++;
                        row.Add($"ContrId{Indexcontrgent}", stagecom.Contragent.Id);
                    row.Add($"ComPositionId{Indexcontrgent}", stagecom.ComPositionId);

                    if (GoodPrice ==null || stagecom.Price <= GoodPrice)
                        GoodPrice = stagecom.Price;
                    row.Add($"ContrPrice{Indexcontrgent}",stagecom.Price);
                        row.Add($"RequestPrice{Indexcontrgent}", stagecom.RequestPrice);
                    row.Add($"ContrStatus{Indexcontrgent}", pos.keys.Id == lastStage.Id ?status:0);



                }
                if (stage == 1 || pos.keys.Id==lastStage.Id)
                    row.Add("GoodPrice", GoodPrice);
                else 
                    row.Add("GoodPrice", null);
                resultData.Add((ExpandoObject)row);
                 

                
                 
            }
            return resultData;
        }
        private static ParticipantStatus  GetStatus(ICollection<StageParticipant> StageParticipants, int contrId)
        {
            var stagepart = StageParticipants.FirstOrDefault(p => p.ContragentId == contrId);
            if (stagepart is null) return ParticipantStatus.FailureParitipate;
            else
                return stagepart.Status;
        }
        private ParticipantStatus getFailureStatus(ContragentDto contragent)
        {
            return ParticipantStatus.FailureParitipate;
        }
        private IEnumerable<ForTableHeader> GetHeaders(IEnumerable<ComStageDto> comStageDtos)
        {

            var contragents = from s in comStageDtos
                              from c in s.StageCompositions//.Where(sc=>s.StageParticipants.Any(p=>p.ContragentId== sc.ContragentId))
                              group s by c.Contragent into contrs

                              orderby contrs.Key.Name
                              select contrs.Key;
            var Participants = from c in comStageDtos.GroupBy(r => r.ComOffer).Select(g => g.Key).FirstOrDefault().ComParticipants
                               select new
                               {
                                   id = c.ContragentId,
                                   status = (comStageDtos
                                          .Where(s => s.StageParticipants.Any(sp => sp.ContragentId == c.ContragentId))
                                          .OrderBy(o => o.Number)
                                          .LastOrDefault())?
                                          .StageParticipants
                                          .FirstOrDefault(sp => sp.ContragentId == c.ContragentId)
                                          .Status
                               };
            var status = from s in contragents
                         join p in Participants on s.Id equals p.id into ps
                         from p in ps.DefaultIfEmpty()
                         select new ForTableHeader
                         {
                             Contragent = s,
                             ParticipantStatus = p == null
                             ? ParticipantStatus.FailureParitipate.ToDescriptionString()
                             : p.status != null ? p.status.ToDescriptionString() : getFailureStatus(s).ToDescriptionString()
                             
                             //:  (p.status == ParticipantStatus.PriceRequest || p.status==ParticipantStatus.PriceConfirmed
                             //   ? ParticipantStatus.Participates.ToDescriptionString()
                             //   : ParticipantStatus.NotParticipate.ToDescriptionString())
                         };
            return status;
        }
         
    }
}
