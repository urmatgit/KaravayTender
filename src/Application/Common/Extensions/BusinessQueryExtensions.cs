// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Common.Extensions
{
   public static class BusinessQueryExtensions
    {
        public static async Task<bool>  CheckPartipiatnIsWinning(this IApplicationDbContext _context, int stageId, int ContragentId,int comOfferId, CancellationToken cancellationToken)
        {
             var data1=await _context.StageParticipants.FindAsync(new object[] { stageId, ContragentId, comOfferId }, cancellationToken);
            var data2 =  _context.ComStages.Where(x => x.ComOfferId == comOfferId).OrderByDescending(o => o.Number).FirstOrDefault();


            return data1.ComStageId==data2.Id;

        }
        public static IQueryable<StagesCrossDto> GetFullInfoForCrossData(this IApplicationDbContext _context, IQueryable<ParticipantCrossDto> participantCrosses)
        {
            var stage = from a in participantCrosses
                        join s in _context.StageCompositions on new { StageId = a.ComStageId, ContrId = a.ContragentId } equals new { StageId = s.ComStageId, ContrId = s.ContragentId }
                        join p in _context.ComPositions on s.ComPositionId equals p.Id into pos
                        from p1 in pos.DefaultIfEmpty()
                        join n in _context.Nomenclatures on p1.NomenclatureId equals n.Id into nom
                        from n1 in nom.DefaultIfEmpty()
                        join c in _context.Contragents on a.ContragentId equals c.Id into contr
                        from c1 in contr.DefaultIfEmpty()
                      //  orderby c1.Name
                        select new StagesCrossDto
                        {
                            ComOfferId = a.ComOfferId,
                            ContragentId = a.ContragentId,
                            ContragentName = c1.Name,
                            ComStageId = a.ComStageId,
                            Number = a.Number,
                            DeadlineDate = a.DeadlineDate,
                            Status = a.Status,
                            Description = a.Description,
                            ComPositionId = s.ComPositionId,
                            NomenclatureName = n1.Name,
                            Price = s.Price,
                            RequestPrice = s.RequestPrice

                        };
            return stage;
        }
        public static IQueryable<ParticipantCrossDto> GetParicipantsForAllStage(this IApplicationDbContext _context, int comOfferId,int? contragentId=null)
        {
            
            var dataStep2 = from a in _context.ComStages 
                            where a.ComOfferId==comOfferId  
                            select new
                            {
                                Number=a.Number,
                                ComStageID = a.Id,
                                DeadlineDate = a.DeadlineDate
                            };
            var dataStep3 = from a in dataStep2
                            join b in _context.StageParticipants on  a.ComStageID equals  b.ComStageId
                            where  (contragentId != null ? b.ContragentId == contragentId : true)
                            select new ParticipantCrossDto
                            {
                                ComOfferId = comOfferId,
                                ContragentId = b.ContragentId,
                                Number = a.Number,
                                ComStageId = a.ComStageID,
                                DeadlineDate = a.DeadlineDate,
                                Status = b.Status,
                                Description = b.Description
                            };
            return dataStep3;
        }
        public static IQueryable<ParticipantCrossDto> GetParicipantsForParcipantPage(this IApplicationDbContext _context,int comOfferId)
        {
            //var data1=_context.ComParticipants
            //          .Include(x=>x.ComOffer)
            //          .ThenInclude(x=>x.StageParticipants.Where(y=>x.ComParticipants.Any(a=>a.ComOfferId== y.ContragentId)))
            //          .ThenInclude(x=>x.ComStage)
            //          .Where(x=>x.ComOfferId==12)
            //          .Select(x=>new
            //          {

            //          })
            var dataStep1N1 = from c in _context.ComParticipants
                              where c.ComOfferId == comOfferId
                              from s in _context.StageParticipants.Where(b => b.ComOfferId == c.ComOfferId && b.ContragentId == c.ContragentId).DefaultIfEmpty()
                              from cs in _context.ComStages.Where(x => x.Id == s.ComStageId).DefaultIfEmpty()
                              group new { c, cs } by new { c.ComOfferId, c.ContragentId } into gr

                              select new ParticipantCrossDto
                              {
                                  ComOfferId = gr.Key.ComOfferId,
                                  ContragentId = gr.Key.ContragentId,
                                  Number = gr.Max(x => x.cs == null ? 0 : x.cs.Number),
                              };
            var data2 = from a in dataStep1N1
                        join b in _context.ComStages on new { comID = a.ComOfferId, num = a.Number } equals new { comID = b.ComOfferId, num = b.Number } into gr
                        from b1 in gr.DefaultIfEmpty()
                        select new ParticipantCrossDto
                        {
                            ComOfferId = a.ComOfferId,
                            ContragentId =a.ContragentId,
                            Number = a.Number,
                            
                            ComStageId =b1!=null? b1.Id : default(int),
                            DeadlineDate = b1 != null ? b1.DeadlineDate: default(DateTime)
                        };
            var dataStep3 = from a in data2
                            join b in _context.StageParticipants on new { Id = a.ComStageId, ContrId = a.ContragentId } equals new { Id = b.ComStageId, ContrId = b.ContragentId } into gr
                            from b1 in gr.DefaultIfEmpty()
                            select new ParticipantCrossDto
                            {
                                ComOfferId = a.ComOfferId,
                                ContragentId = a.ContragentId,
                                Number = a.Number,
                                ComStageId = a.ComStageId,
                                DeadlineDate = a.DeadlineDate,
                                Status = b1!=null? b1.Status : Domain.Enums.ParticipantStatus.Request ,
                                Description =b1!=null? b1.Description: ""
                            };
            

            return dataStep3;
        }
        public static IQueryable<ParticipantCrossDto> GetParicipantsForLastStage(this IApplicationDbContext _context, int comOfferId, int? contragentId = null)
        {

            

            //
            var dataStep1 = from c in _context.ComStages
                            join p in _context.StageParticipants on c.Id equals p.ComStageId
                            where c.ComOfferId == comOfferId 
                            group c by new { c.ComOfferId, p.ContragentId } into gr
                            select new
                            {
                                ComOfferId = gr.Key.ComOfferId,
                                ContragentId = gr.Key.ContragentId,
                                
                                Number = gr.Max(m => m.Number)

                            };
            var dataStep2 = from a in dataStep1
                            join c in _context.ComStages on new { Id = a.ComOfferId, Number = a.Number } equals new { Id = c.ComOfferId, Number = c.Number }
                            select new
                            {
                                a,
                                ComStageID = c.Id,
                                DeadlineDate = c.DeadlineDate
                            };
            var dataStep3 = from a in dataStep2
                            join b in _context.StageParticipants on new { Id = a.ComStageID, ContrId = a.a.ContragentId } equals new { Id = b.ComStageId, ContrId = b.ContragentId }
                            
                            where (contragentId != null ? b.ContragentId == contragentId : true)
                            select new ParticipantCrossDto
                            {
                                ComOfferId = a.a.ComOfferId,
                                ContragentId = b.ContragentId,
                                Number = a.a.Number,
                                ComStageId = a.ComStageID,
                                DeadlineDate = a.DeadlineDate,
                                Status = b.Status,
                                Description = b.Description
                            };
            return dataStep3;
        }
        public static IQueryable<ParticipantCrossDto> GetComOfferWithLastStage(this IApplicationDbContext _context, int? comOfferId=null, int? contragentId = null, Domain.Enums.ParticipantStatus? status=null)
        {



            //
            var dataStep1 = from c in _context.ComStages
                            join p in _context.StageParticipants on c.Id equals p.ComStageId
                            where  (comOfferId !=null? c.ComOfferId == comOfferId : true) && (contragentId != null ? p.ContragentId == contragentId : true)
                            group c by new { c.ComOfferId, p.ContragentId } into gr
                            select new
                            {
                                ComOfferId = gr.Key.ComOfferId,
                                ContragentId = gr.Key.ContragentId,

                                Number = gr.Max(m => m.Number)

                            };
            var dataStep2 = from a in dataStep1
                            join c in _context.ComStages on new { Id = a.ComOfferId, Number = a.Number } equals new { Id = c.ComOfferId, Number = c.Number }
                            select new
                            {
                                a,
                                ComStageID = c.Id,
                                DeadlineDate = c.DeadlineDate
                            };
            var dataStep3 = from a in dataStep2
                            join b in _context.StageParticipants on new { Id = a.ComStageID, ContrId = a.a.ContragentId } equals new { Id = b.ComStageId, ContrId = b.ContragentId }
                            join c in _context.ComOffers on a.a.ComOfferId equals c.Id
                            where (status==null? true : b.Status==status) //Domain.Enums.ParticipantStatus.Request)
                            select new ParticipantCrossDto
                            {
                                ComOfferId = a.a.ComOfferId,
                                ComOfferStatus=c.Status,
                                ContragentId = b.ContragentId,
                                Number = a.a.Number,
                                ComStageId = a.ComStageID,
                                DeadlineDate = a.DeadlineDate,
                                Status = b.Status,
                                Description = b.Description
                            };
            return dataStep3;
        }
        public static IQueryable<StagesCrossDto> GetParicipantsForLastStageWithInfo(this IApplicationDbContext _context, int comOfferId, int? contragentId = null)
        {
            var data1 = _context.GetParicipantsForLastStage(comOfferId, contragentId);
            return _context.GetFullInfoForCrossData(data1);
        }
    }
}
