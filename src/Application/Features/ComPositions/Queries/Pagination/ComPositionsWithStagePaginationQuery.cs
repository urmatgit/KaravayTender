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

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Queries.Pagination
{
    public class ComPositionsWithStagePaginationQuery : PaginationRequest, IRequest<PaginatedData<ComPositionDto>>
    {
        public int ComOfferId { get; set; }
        /// <summary>
        /// 0-All
        /// 1- Last stage
        /// </summary>
        public int IsLastStage { get; set; }
    }
    
    public class ComPositionsWithStagePaginationQueryHandler :
         IRequestHandler<ComPositionsWithStagePaginationQuery, PaginatedData<ComPositionDto>>
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

        public async Task<PaginatedData<ComPositionDto>> Handle(ComPositionsWithStagePaginationQuery request, CancellationToken cancellationToken)
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
            if (request.IsLastStage == 1)
            {
                var dataLast = await _mediator.Send(new GetByStageLastQuery() { ComOfferId = request.ComOfferId }, cancellationToken);
            }
            var filters = PredicateBuilder.FromFilter<ComPosition>(request.FilterRules);
            
                var data = await _context.ComPositions
                    .Specify(new FilterByComOfferQuerySpec(request.ComOfferId,ContragentId))
                     .Where(filters)
                     .Include(n => n.Nomenclature)
                     .ThenInclude(n => n.Category)
                     .Include(a=>a.AreaComPositions)
                     .ThenInclude(a=>a.Area)
                    .Include(n => n.Nomenclature)
                    .ThenInclude(n => n.UnitOf)
                    .Include(n => n.Nomenclature)
                    .ThenInclude(n => n.Vat)
                    .Include(n => n.Nomenclature)
                    .ThenInclude(n => n.NomenclatureQualityDocs)
                    .ThenInclude(d=>d.QualityDoc)
                    .Include(s=>s.StageCompositions)
                    
                    .OrderBy($"{request.Sort} {request.Order}")
                    
                    .PaginatedDataAsync(request.Page, request.Rows);
                  //.ProjectTo<ComPositionDto>(_mapper.ConfigurationProvider)
                  var datDto = _mapper.Map<IEnumerable<ComPositionDto>>(data.rows);
                return new PaginatedData<ComPositionDto>(datDto, data.total); ;
             
        }
        public class FilterByComOfferQuerySpec : Specification<ComPosition>
        {
            public FilterByComOfferQuerySpec(int comOfferId,int contrgentid,int stage=0)
            {
                if (stage==0)
                    Criteria = p => p.ComOfferId == comOfferId && p.StageCompositions.Any(s=>s.ContragentId==contrgentid);
                else
                    Criteria = p => p.ComOfferId == comOfferId && p.StageCompositions.Any(s => s.ContragentId == contrgentid && s.ComStage.Number==stage);
            }


        }
    }
}

