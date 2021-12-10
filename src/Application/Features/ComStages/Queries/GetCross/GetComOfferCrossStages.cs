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
using CleanArchitecture.Razor.Domain.Entities.Karavay;
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
            if (request.Stage == 1)
            {
                var dataLast = await _mediator.Send(new GetByStageLastQuery() { ComOfferId = request.ComOfferId }, cancellationToken);
                if (dataLast != null)
                {
                    queryResult.Add(dataLast);
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
                result.Body = GetBody(queryResult);
            }
            
           //  List<ExpandoObject> sampleObjects = new List<ExpandoObject>();
            //dynamic row1 = new ExpandoObject() 
            //row1.NomName = "Позиция1";
            //row1.Stage = 1;
            //row1.ContName1 = "Контрагет21";
            //row1.ContrId1 = "1";
            //row1.ContrPrice1 = "100";
            //row1.ContrStatus1 = "true";
            //sampleObjects.Add(row1);

            //dynamic row2 = new ExpandoObject();
            //row2.NomName = "Позиция2";
            //row2.Stage = 1;
            //row2.ContName1 = "Контрагет22";
            //row2.ContrId1 = "2";
            //row2.ContrPrice1 = "110";
            //row2.ContrStatus1 = "false";
            
            //sampleObjects.Add(row2);
            
            
            return Result<ComStageCrossDTO>.Success(result) ;
        }
        private List<ExpandoObject> GetBody(IEnumerable<ComStageDto> dataDto)
        {
            //var Nomenclatures = result.StageCompositions.GroupBy(c => new { c.ComPosition, c.ComStage }).Select(c => new
            //{
            //    ComPosition = c.Key.ComPosition,
            //    comStage = c.Key.ComStage,
            //    stageNumber = result.Number,
            //    participients = c.ToList(),

            //});
            List<ExpandoObject> resultData = new List<ExpandoObject>();

            var postions = dataDto.SelectMany(
                                        c => c.StageCompositions.Select(p => new { position =  p.ComPosition, child = c, stages = p.ComStage })
                                    ).GroupBy(cs =>   cs.position  )
                                    .Select(g => new
                                    {
                                        keys = g.Key,
                                        stagecoms = g.Select(cp => cp.child).OrderBy(o=>o.Number).ToList()
                                    }).OrderBy(o =>  o.keys.Nomenclature.Name);

            int Indexcontrgent = 0;                                          
            foreach(var pos in postions)
            {
                
                
                foreach (var stage in pos.stagecoms)
                {
                    var row = new ExpandoObject() as IDictionary<string, object>;
                    row.Add($"NomName", pos.keys.Nomenclature.Name);
                    row.Add($"Stage", stage.Number);
                    foreach (var stagecom in stage.StageCompositions)
                    {
                        Indexcontrgent++;
                        row.Add($"ContrId{Indexcontrgent}", stagecom.Contragent.Id);
                        row.Add($"ContrPrice{Indexcontrgent}",stagecom.Price);
                        row.Add($"ContrStatus{Indexcontrgent}", stagecom.Status);
                        
                    }
                    resultData.Add((ExpandoObject)row);
                }

                
                 
            }
            return resultData;
        }
        private IEnumerable<ForTableHeader> GetHeaders(IEnumerable<ComStageDto> dataDto)
        {
            
            var contragents = from s in dataDto
                              from c in s.StageCompositions
                              group s by c.Contragent into contrs
                              orderby contrs.Key.Name
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
            return status;
        }
         
    }
}
