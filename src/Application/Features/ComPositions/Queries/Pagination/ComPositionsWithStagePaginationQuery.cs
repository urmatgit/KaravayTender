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
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Application.Common.Specification;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Razor.Domain.Identity;
using CleanArchitecture.Razor.Application.Features.ComStages.Queries.GetBy;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Queries.Pagination
{
    public class ComPositionsWithStagePaginationQuery : PaginationRequest, IRequest<PaginatedData<ComPositionDtoEx>>
    {
        public int ComOfferId { get; set; }
        /// <summary>
        /// 0-All
        /// 1- Last stage
        /// </summary>
        public int IsLastStage { get; set; }
    }
    
    public class ComPositionsWithStagePaginationQueryHandler :
         IRequestHandler<ComPositionsWithStagePaginationQuery, PaginatedData<ComPositionDtoEx>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ComPositionsWithStagePaginationQueryHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        private readonly ILogger<ComPositionsWithStagePaginationQueryHandler> _logger;
        public ComPositionsWithStagePaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IMediator mediator,
               ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
             ILogger<ComPositionsWithStagePaginationQueryHandler> logger,
            IStringLocalizer<ComPositionsWithStagePaginationQueryHandler> localizer
            )
        {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _logger = logger;
        }
        public static string GetAreas(ICollection<AreaComPosition> areaComPositions)
        {
            return string.Join(", ", areaComPositions.Select(n => n.Area != null ? n.Area.Name : ""));
        }
        public async Task<PaginatedData<ComPositionDtoEx>> Handle(ComPositionsWithStagePaginationQuery request, CancellationToken cancellationToken)
        {
            int ContragentId = 0;
            var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (currentUser is null)
            {
                throw new Exception("Пользователь не найден!");
            }
            if (currentUser.ContragentId.HasValue)
            {
                ContragentId = currentUser.ContragentId.Value;

            }
            else
            {
                var contragent = await _context.Contragents.FirstOrDefaultAsync(c => c.ApplicationUserId == currentUser.Id, cancellationToken);
                if (contragent is null)
                    throw new Exception("Контрагент не найден!");
                ContragentId = contragent.Id;
            }
            ComStageDto LastStage = null;
            var filters = PredicateBuilder.FromFilter<ComPositionDtoEx>(request.FilterRules, new List<string>() { "ParticipantStatusStr" });
            IQueryable<ParticipantCrossDto> participantCrosses  = null;
            if (request.IsLastStage == 1)
                participantCrosses=_context.GetParicipantsForLastStage(request.ComOfferId, ContragentId);
            else
                participantCrosses= _context.GetParicipantsForAllStage(request.ComOfferId, ContragentId);
            var stage = _context.GetFullInfoForCrossData(participantCrosses);

          //  if (request.IsLastStage == 1)
            {
                
                
                

                //.Include(z => z.Category)
                //                        .Include(z => z.Nomenclature)
                //                        .ThenInclude(x => x.Vat)
                //                        .Include(z => z.AreaComPositions)
                //                        .ThenInclude(x => x.Area)
                //var res1 = from s in stage
                //           join p in _context.ComPositions on s.ComPositionId equals p.Id
                //           join ac in _context.AreaComPositions on s.ComPositionId equals ac.ComPositionId into ar
                //           from ac1 in ar.DefaultIfEmpty()
                //           join a in _context.Areas on ac1.AreaId equals a.Id into ag
                //           from a1 in ag.DefaultIfEmpty()
                //           group new { s, a1 } by new { s, p } into gr
                //           select new ComPositionDtoEx
                //           {
                //               Id = gr.Key.p.Id,
                //               CategoryId = gr.Key.p.CategoryId,
                //               CategoryName = gr.Key.p.Category.Name,
                //               DeliveryCount = gr.Key.p.DeliveryCount,
                //               Volume = gr.Key.p.Volume,
                //               AddRequirement = gr.Key.p.AddRequirement,
                //               Stage = gr.Key.s.Number,
                //               ParticipantStatus = gr.Key.s.Status,
                //               StageId = gr.Key.s.ComStageId,
                //               NomenclatureId = gr.Key.p.NomenclatureId,
                //               NomName = gr.Key.p.Nomenclature.Name,
                //               UnitOfName = gr.Key.p.Nomenclature.UnitOf.Name,
                //               NomVolume = gr.Key.p.Nomenclature.Volume,
                //               NomSpecification = gr.Key.p.Nomenclature.Specifications,

                //               RequestPrice = gr.Key.s.RequestPrice,
                //               InputPrice = gr.Key.s.Price,
                //               NomStavka = gr.Key.p.Nomenclature.Vat.Stavka,
                //             //  AreaNames = gr.Select(x => x.a1.Id).Count().ToString()   //string.Join(',', gr.Select(g=>g.a1.Name).ToList())
                //           };


                var result=await  _context.ComPositions
                                  //.Include(x=>x.Nomenclature)
                                  //.ThenInclude(x=>x.NomenclatureQualityDocs)
                                  //.ThenInclude(x=>x.QualityDoc)
                                  
                                  .Join(stage,p=>p.Id,s=>s.ComPositionId,(p,s)=> new ComPositionDtoEx
                           {
                               Id=s.ComPositionId,
                               CategoryId=p.CategoryId,
                               CategoryName=p.Category.Name,
                               DeliveryCount=p.DeliveryCount,
                               Volume=p.Volume,
                               AddRequirement=p.AddRequirement,
                               Stage=s.Number,
                               ParticipantStatus=s.Status,
                               StageId=s.ComStageId,
                               NomenclatureId=p.NomenclatureId,
                               NomName=p.Nomenclature.Name,
                               UnitOfName=p.Nomenclature.UnitOf.Name,
                               NomVolume=p.Nomenclature.Volume,
                               NomSpecification=p.Nomenclature.Specifications,
                               
                               RequestPrice=s.RequestPrice,
                               InputPrice=s.Price,
                               NomStavka=p.Nomenclature.Vat.Stavka,
                               //AreaNames =string.Join(',', p.AreaComPositions.Select(a=>a.Area.Name).ToList())
                              // QualityDocsNames =p.Nomenclature.NomenclatureQualityDocs != null && p.Nomenclature.NomenclatureQualityDocs.Count > 0 ? string.Join(", ", p.Nomenclature.NomenclatureQualityDocs.Select(n => n.QualityDoc.Name)) : ""
                           })
                    .Where(filters)
                    .PaginatedDataLazySortAsync(request.Page, request.Rows, request.Sort, request.Order);

                return PaginatedData<ComPositionDtoEx>.CreateWithCheckSort(result.rows, result.total, result.IsSorted, request.Sort, request.Order);
                #region OldVersion
                //LastStage = await _mediator.Send(new GetByStageLastDtoQuery() { ComOfferId = request.ComOfferId }, cancellationToken);



                //var data = await _context.ComPositions
                //    .Specify(new FilterByComOfferQuerySpec(request.ComOfferId, ContragentId, (request.IsLastStage == 1 ? LastStage.Number : 0)))
                //     .Where(filters)

                //    .Include(s => s.StageCompositions)
                //    .ThenInclude(s => s.ComStage)
                //    .OrderByWithCheck(request.Sort, request.Order)
                //    .Select(c =>
                //    new ComPositionDtoEx
                //    {
                //        Id = c.Id,

                //        CategoryId = c.CategoryId,
                //        CategoryName = c.Category.Name,
                //        DeliveryCount = c.DeliveryCount,
                //        Volume = c.Volume,
                //        AddRequirement = c.AddRequirement,
                //        Stage = LastStage == null ? 0 : LastStage.Number,
                //        ParticipantStatus = LastStage == null ? ParticipantStatus.PriceConfirmed : GetParticipant(LastStage, ContragentId),
                //    //GetParticipant(s,ContragentId,request.ComOfferId),  //s.ComStage.StageParticipants.FirstOrDefault(f => f.ContragentId == ContragentId && f.ComOfferId == request.ComOfferId).Status ,
                //    StageId = LastStage == null ? 0 : LastStage.Id,
                //        NomenclatureId = c.NomenclatureId,
                //        NomName = c.Nomenclature.Name,
                //        UnitOfName = c.Nomenclature.UnitOf.Name,
                //        NomVolume = c.Nomenclature.Volume,
                //        NomSpecification = c.Nomenclature.Specifications,
                //        AreaNames = c.AreaComPositions != null ? string.Join(", ", c.AreaComPositions.Select(n => n.Area != null ? n.Area.Name : "")) : "",
                //        RequestPrice = LastStage == null ? false : GetPrices(LastStage, ContragentId, c.Id).RequestPrice,
                //        InputPrice = LastStage == null ? null : GetPrices(LastStage, ContragentId, c.Id).Price,
                //        NomStavka = c.Nomenclature.Vat.Stavka,

                //        QualityDocsNames = c.Nomenclature.NomenclatureQualityDocs != null && c.Nomenclature.NomenclatureQualityDocs.Count > 0 ? string.Join(", ", c.Nomenclature.NomenclatureQualityDocs.Select(n => n.QualityDoc.Name)) : ""
                //    })

                //    .PaginatedDataLazySortAsync(request.Page, request.Rows, request.Sort, request.Order);
                ////.Distinct()
                ////.OrderBy($"{request.Sort} {request.Order}")
                ////.ToListAsync(cancellationToken);
                ////.PaginatedDataAsync(request.Page, request.Rows);

                ////.ProjectTo<ComPositionDto>(_mapper.ConfigurationProvider)
                ////var datDto = _mapper.Map<IEnumerable<ComPositionDtoEx>>(data.rows);
                //return PaginatedData<ComPositionDtoEx>.CreateWithCheckSort(data.rows, data.total, data.IsSorted,request.Sort,request.Order);
                #endregion
            }
            //else
            //{


            //    var datas = _context.GetParicipantsForAllStage(request.ComOfferId, ContragentId);
            //    var stage = _context.GetFullInfoForCrossData(datas);


            //    var data = await _context.StageCompositions
            //        .Specify(new FilterByComOfferWithStageQuerySpec(request.ComOfferId, ContragentId))
            //        .Where(filters)
            //        .OrderByWithCheck(request.Sort, request.Order)
            //        .Select(c => new ComPositionDtoEx()
            //        {
            //            Id = c.ComPositionId,

            //            CategoryId = c.ComPosition.CategoryId,
            //            CategoryName = c.ComPosition.Category.Name,
            //            DeliveryCount = c.ComPosition.DeliveryCount,
            //            Volume = c.ComPosition.Volume,
            //            AddRequirement = c.ComPosition.AddRequirement,
            //            Stage = c.ComStage.Number,
            //            ParticipantStatus =c.ComStage.StageParticipants.First(s=>s.ContragentId==ContragentId).Status,
            //            //GetParticipant(s,ContragentId,request.ComOfferId),  //s.ComStage.StageParticipants.FirstOrDefault(f => f.ContragentId == ContragentId && f.ComOfferId == request.ComOfferId).Status ,
            //            StageId =c.ComStageId,
            //            NomenclatureId = c.ComPosition.NomenclatureId,
            //            NomName = c.ComPosition.Nomenclature.Name,
            //            UnitOfName = c.ComPosition.Nomenclature.UnitOf.Name,
            //            NomVolume = c.ComPosition.Nomenclature.Volume,
            //            NomSpecification = c.ComPosition.Nomenclature.Specifications,
            //            AreaNames = c.ComPosition.AreaComPositions != null ? string.Join(", ", c.ComPosition.AreaComPositions.Select(n => n.Area != null ? n.Area.Name : "")) : "",
            //            RequestPrice =c.RequestPrice,//  LastStage == null ? false : GetPrices(LastStage, ContragentId, c.Id).RequestPrice,
            //            InputPrice =c.Price, //   LastStage == null ? null : GetPrices(LastStage, ContragentId, c.Id).Price,
            //            NomStavka = c.ComPosition.Nomenclature.Vat.Stavka,

            //            QualityDocsNames = c.ComPosition.Nomenclature.NomenclatureQualityDocs != null && c.ComPosition.Nomenclature.NomenclatureQualityDocs.Count > 0 ? string.Join(", ", c.ComPosition.Nomenclature.NomenclatureQualityDocs.Select(n => n.QualityDoc.Name)) : ""

            //        })
            //            // .OrderBy($"{request.Sort} {request.Order}")
            //          // .PaginatedDataAsync(request.Page, request.Rows);



            //        .PaginatedDataLazySortAsync(request.Page, request.Rows, request.Sort, request.Order);
            //    //var datDto = _mapper.Map<IEnumerable<ComPositionDtoEx>>(data.rows);
            //    return PaginatedData<ComPositionDtoEx>.CreateWithCheckSort(data.rows, data.total, data.IsSorted, request.Sort, request.Order);
            //    //return new PaginatedData<ComPositionDtoEx>(datDto, datDto.Count());
            //    //TODO for all step
            //}
            //return data;

        }
        private static StageCompositionDto GetPrices(ComStageDto comStage, int ContragentId,int compostionId)
        {
            return comStage.StageCompositions.FirstOrDefault(i => i.ContragentId == ContragentId && i.ComPositionId == compostionId);
        }
        private static ParticipantStatus GetParticipant(ComStageDto  comStage, int ContragentId)
        {
            var StageParticipants = comStage.StageParticipants ?? null;
            if (StageParticipants is not null)
            {
                var StageParticipant = comStage.StageParticipants.FirstOrDefault(f => f.ContragentId == ContragentId);
                if (StageParticipant is not null)
                    return StageParticipant.Status;
                return ParticipantStatus.Excluded;

            }
            else
                return ParticipantStatus.Excluded;
        }
        public class FilterByComOfferWithStageQuerySpec : Specification<StageComposition>
        {
            public FilterByComOfferWithStageQuerySpec(int comOfferId, int contrgentid)
            {
                
                    Criteria = p => p.ComStage.ComOfferId == comOfferId && p.ContragentId == contrgentid;
                //&& s.ComStage.StageParticipants.Any(st => st.Status != Domain.Enums.ParticipantStatus.FailureParitipate || st.Status == Domain.Enums.ParticipantStatus.Excluded));
            }


        }
        public class FilterByComOfferQuerySpec : Specification<ComPosition>
        {
            public FilterByComOfferQuerySpec(int comOfferId,int contrgentid,int stage=0)
            {
                if (stage == 0)
                    Criteria = p => p.ComOfferId == comOfferId && p.StageCompositions.Any(s => s.ContragentId == contrgentid);
                //&& s.ComStage.StageParticipants.Any(st => st.Status != Domain.Enums.ParticipantStatus.FailureParitipate && st.Status == Domain.Enums.ParticipantStatus.Excluded));

                else
                    Criteria = p => p.ComOfferId == comOfferId && p.StageCompositions.Any(s => s.ContragentId == contrgentid && s.ComStage.Number == stage);
                    //&& s.ComStage.StageParticipants.Any(st => st.Status != Domain.Enums.ParticipantStatus.FailureParitipate || st.Status == Domain.Enums.ParticipantStatus.Excluded));
            }


        }
    }
}

