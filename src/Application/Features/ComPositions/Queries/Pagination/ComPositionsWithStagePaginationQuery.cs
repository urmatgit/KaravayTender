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

            if (request.IsLastStage == 1)
            {
                LastStage = await _mediator.Send(new GetByStageLastDtoQuery() { ComOfferId = request.ComOfferId }, cancellationToken);
            }
            var filters = PredicateBuilder.FromFilter<ComPosition>(request.FilterRules);
            //var data1 = await _context.ComPositions
            //   .Specify(new FilterByComOfferQuerySpec(request.ComOfferId, ContragentId, (request.IsLastStage == 1 ? LastStage.Number : 0)))
            //   .Include(a => a.AreaComPositions)
            //     .ThenInclude(a => a.Area)
            //    .Where(filters)
            //    .ToListAsync(cancellationToken);
            //var data1 = await _context.ComPositions
            //   .Specify(new FilterByComOfferQuerySpec(request.ComOfferId, ContragentId, (request.IsLastStage == 1 ? LastStage.Number : 0)))
            //    .Where(filters)
            //    .GroupJoin(_context.StageCompositions.Where(s => s.ContragentId == ContragentId && s.ComStageId == LastStage.Id)
            //    .ToListAsync(cancellationToken);

            var data = await _context.ComPositions
                .Specify(new FilterByComOfferQuerySpec(request.ComOfferId, ContragentId, (request.IsLastStage == 1 ? LastStage.Number : 0)))
                 .Where(filters)
                // .Include(n => n.Nomenclature)
                // .ThenInclude(n => n.Category)
                //.Include(a => a.AreaComPositions)
                //.ThenInclude(a => a.Area)
                //.Include(n => n.Nomenclature)
                //.ThenInclude(n => n.UnitOf)
                //.Include(n => n.Nomenclature)
                //.ThenInclude(n => n.Vat)
                //.Include(n => n.Nomenclature)
                //.ThenInclude(n => n.NomenclatureQualityDocs)
                //.ThenInclude(d => d.QualityDoc)
                .Include(s => s.StageCompositions)
                .ThenInclude(s=>s.ComStage)

                .Select(c=>
                new ComPositionDtoEx
                {
                    Id = c.Id,

                    CategoryId = c.CategoryId,
                    CategoryName = c.Category.Name,
                    DeliveryCount = c.DeliveryCount,
                    Volume = c.Volume,
                    AddRequirement = c.AddRequirement,
                    Stage = LastStage.Number,
                    ParticipantStatus = GetParticipant(LastStage, ContragentId),
                    //GetParticipant(s,ContragentId,request.ComOfferId),  //s.ComStage.StageParticipants.FirstOrDefault(f => f.ContragentId == ContragentId && f.ComOfferId == request.ComOfferId).Status ,
                    StageId = LastStage.Id,
                    NomenclatureId = c.NomenclatureId,
                    NomName = c.Nomenclature.Name,
                    UnitOfName = c.Nomenclature.UnitOf.Name,
                    NomVolume = c.Nomenclature.Volume,
                    NomSpecification = c.Nomenclature.Specifications,
                    AreaNames = c.AreaComPositions !=null ? string.Join(", ", c.AreaComPositions.Select(n => n.Area != null ? n.Area.Name : "")):"",
                    RequestPrice = GetPrices(LastStage,ContragentId,c.Id). RequestPrice,
                    InputPrice = GetPrices(LastStage, ContragentId, c.Id).Price,
                    NomStavka = c.Nomenclature.Vat.Stavka,

                    QualityDocsNames = c.Nomenclature.NomenclatureQualityDocs !=null && c.Nomenclature.NomenclatureQualityDocs.Count>0 ? string.Join(", ", c.Nomenclature.NomenclatureQualityDocs.Select(n => n.QualityDoc.Name)):""
                })
                  
                   //.Distinct()
                   .OrderBy($"{request.Sort} {request.Order}")

                    .PaginatedDataAsync(request.Page, request.Rows);
                    
            //.ProjectTo<ComPositionDto>(_mapper.ConfigurationProvider)
            //var datDto = _mapper.Map<IEnumerable<ComPositionDtoEx>>(data.rows);
            //return new PaginatedData<ComPositionDtoEx>(data, data.total); ;
            return data;
            
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

