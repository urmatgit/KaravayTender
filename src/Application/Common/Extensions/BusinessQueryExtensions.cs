// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;

namespace CleanArchitecture.Razor.Application.Common.Extensions
{
   public static class BusinessQueryExtensions
    {
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
        public static IQueryable<ParticipantCrossDto> GetParicipantsForLastStage(this IApplicationDbContext _context, int comOfferId, int? contragentId = null)
        {
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
    }
}
