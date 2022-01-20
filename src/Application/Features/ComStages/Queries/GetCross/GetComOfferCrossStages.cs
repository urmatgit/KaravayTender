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
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
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
        private async Task<IEnumerable<StagesCrossDto>> GetDataAllStage(int comofferid, CancellationToken cancellationToken)
        {
            var datas = _context.GetParicipantsForAllStage(comofferid);
            var stage = _context.GetFullInfoForCrossData(datas);
            var result = stage.OrderBy(o => o.NomenclatureName)
                       .ThenBy(o => o.Number)
                       .ThenBy(o => o.ContragentName);
            return await result.ToListAsync(cancellationToken);
        }
        private async Task<ComStageCrossDTO> GetCrossDataAllStagePage(int comOfferId, CancellationToken cancellationToken)
        {
            ComStageCrossDTO result = new ComStageCrossDTO();
            var lastStageData = await GetDataAllStage(comOfferId, cancellationToken);
            var lastStageNumber = lastStageData.OrderBy(o => o.Number).LastOrDefault();
            var contrgents = from s in lastStageData
                             group s by s.ContragentName into contr
                             orderby contr.Key
                             select new
                             {
                                 caption= $"{contr.Key}",
                                 number = contr.Max(x => x.Number),
                                 status=contr.Select(x=>x.Status).LastOrDefault()
                             };
            result.Header = contrgents.Select(x=> $"{x.caption} ({x.status.ToDescriptionString()})").ToArray();
            List<ExpandoObject> resultData = new List<ExpandoObject>();
            
            var listPosition = lastStageData.GroupBy(g => new { g.ComPositionId, g.ComStageId });
            foreach (var position in listPosition)
                foreach (var nomenclture in lastStageData.Where(p => p.ComPositionId == position.Key.ComPositionId && p.ComStageId==position.Key.ComStageId).Select(n => new {
                    n.NomenclatureName,
                    n.DeadlineDate,
                    n.ComPositionId,
                    n.ComStageId,
                    n.Number
                }).Distinct())
                {


                    var row = new ExpandoObject() as IDictionary<string, object>;
                    row.Add($"NomName", nomenclture.NomenclatureName);

                    row.Add($"Stage", nomenclture.Number);
                    row.Add($"StageId", nomenclture.ComStageId);

                    row.Add($"StageDeadline", nomenclture.DeadlineDate);
                    int Indexcontrgent = 0;
                    decimal? GoodPrice = null;
                    //перечисляем контрагентов
                    foreach (var contr in lastStageData.Where(c => c.ComPositionId == nomenclture.ComPositionId && c.ComStageId==nomenclture.ComStageId))
                    {
                        // var status = GetStatus(pos.stage1.stage.StageParticipants, stagecom.ContragentId);
                        // if (status == ParticipantStatus.FailureParitipate && stage==1) continue;
                        Indexcontrgent++;
                        row.Add($"ContrId{Indexcontrgent}", contr.ContragentId);
                        row.Add($"ComPositionId{Indexcontrgent}", contr.ComPositionId);

                        if (GoodPrice == null || contr.Price <= GoodPrice)
                            GoodPrice = contr.Price;
                        row.Add($"ContrPrice{Indexcontrgent}", contr.Price);
                        row.Add($"RequestPrice{Indexcontrgent}", contr.RequestPrice);
                        row.Add($"ContrStatus{Indexcontrgent}", contr.Status);
                    }
                    if (nomenclture.ComStageId==lastStageNumber.ComStageId)
                        row.Add("GoodPrice", GoodPrice);
                    else 
                        row.Add("GoodPrice", null);
                    resultData.Add((ExpandoObject)row);

                }
            result.Body = resultData;

            return result;

        }
        private async  Task<IEnumerable<StagesCrossDto>> GetDataLastStage(int comofferid, CancellationToken cancellationToken)
        {
            var datas =  _context.GetParicipantsForLastStage(comofferid);
            var stage = _context.GetFullInfoForCrossData(datas);
            #region getFullInfo
            //var stageO = from a in datas
            //            join s in _context.StageCompositions on new { StageId = a.ComStageId, ContrId = a.ContragentId } equals new { StageId = s.ComStageId, ContrId = s.ContragentId }
            //            join p in _context.ComPositions on s.ComPositionId equals p.Id into pos
            //            from p1 in pos.DefaultIfEmpty()
            //            join n in _context.Nomenclatures on p1.NomenclatureId equals n.Id into nom
            //            from n1 in nom.DefaultIfEmpty()
            //            join c in _context.Contragents on a.ContragentId equals c.Id into contr
            //            from c1 in contr.DefaultIfEmpty()
            //            orderby c1.Name
            //            select new StagesCrossDto
            //            {
            //                ComOfferId=a.ComOfferId,
            //                ContragentId=a.ContragentId,
            //                ContragentName=c1.Name,
            //                ComStageId=a.ComStageId,
            //                Number=a.Number,
            //                DeadlineDate=a.DeadlineDate,
            //                Status=a.Status,
            //                Description=a.Description,
            //                ComPositionId=s.ComPositionId,
            //                NomenclatureName=n1.Name,
            //                Price=s.Price,
            //                RequestPrice=s.RequestPrice

            //            };
            //var datas= await stage.ToListAsync(cancellationToken);
            #endregion
            return await stage.ToListAsync(cancellationToken);
        }

        private async Task<ComStageCrossDTO>  GetCrossDataLastStagePage(int comOfferId,CancellationToken cancellationToken)
        {
            ComStageCrossDTO result = new ComStageCrossDTO();
            var lastStageData = await GetDataLastStage(comOfferId, cancellationToken);
            var contrgents = from s in lastStageData
                             group s by new { s.ContragentName, s.Status } into contr
                             select $"{contr.Key.ContragentName} ({contr.Key.Status.ToDescriptionString()})";
            result.Header = contrgents.ToArray();
            // var contrForBody= lastStage. Where(c => c.ComPositionId == nomenclture.ComPositionId)
            List<ExpandoObject> resultData = new List<ExpandoObject>();
            var listPosition = lastStageData.GroupBy(g => g.ComPositionId);
            var lastStageNumber = lastStageData.OrderBy(o => o.Number).LastOrDefault();
            foreach (var position in listPosition)
                foreach (var nomenclture in lastStageData.Where(p => p.ComPositionId == position.Key).Select(n => new {
                    n.NomenclatureName,
                    
                    n.ComPositionId
                }).Distinct())
                {


                    var row = new ExpandoObject() as IDictionary<string, object>;
                    row.Add($"NomName", nomenclture.NomenclatureName);

                    row.Add($"Stage", lastStageNumber.Number);
                    row.Add($"StageId", lastStageNumber.ComStageId);

                    row.Add($"StageDeadline", lastStageNumber.DeadlineDate);
                    int Indexcontrgent = 0;
                    decimal? GoodPrice = null;
                    //перечисляем контрагентов
                    foreach (var contr in lastStageData.Where(c => c.ComPositionId == nomenclture.ComPositionId))
                    {
                        // var status = GetStatus(pos.stage1.stage.StageParticipants, stagecom.ContragentId);
                        // if (status == ParticipantStatus.FailureParitipate && stage==1) continue;
                        Indexcontrgent++;
                        row.Add($"ContrId{Indexcontrgent}", contr.ContragentId);
                        row.Add($"ComPositionId{Indexcontrgent}", contr.ComPositionId);

                        if ((contr.Status!=ParticipantStatus.FailureParitipate &&  contr.Status!=ParticipantStatus.Cancel) && (GoodPrice == null || contr.Price <= GoodPrice))
                            GoodPrice = contr.Price;
                        row.Add($"ContrPrice{Indexcontrgent}", contr.Price);
                        row.Add($"RequestPrice{Indexcontrgent}", contr.RequestPrice);
                        row.Add($"ContrStatus{Indexcontrgent}", contr.Status);
                    }
                    
                    row.Add("GoodPrice", GoodPrice);
                    resultData.Add((ExpandoObject)row);

                }
            result.Body = resultData;
            result.CurrentStage = lastStageNumber?.Number;
            result.CurrentStageId = lastStageNumber?.ComStageId;
            result.DeadlineDate = lastStageNumber?.DeadlineDate;
            return result;
        }
        public async Task<Result<ComStageCrossDTO>> Handle(GetComOfferCrossStages request, CancellationToken cancellationToken)
        {
            ComStageCrossDTO result = new ComStageCrossDTO();
            if (request.Stage == 1) //Последний этап
            {
                result = await GetCrossDataLastStagePage(request.ComOfferId, cancellationToken);
                return Result<ComStageCrossDTO>.Success(result);
            }
            else
            {
                result = await GetCrossDataAllStagePage(request.ComOfferId, cancellationToken);
                return Result<ComStageCrossDTO>.Success(result);
            }

        }
        public async Task<Result<ComStageCrossDTO>> HandleO(GetComOfferCrossStages request, CancellationToken cancellationToken)
        {
             await Handle(request, cancellationToken);


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
