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
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Queries.Pagination
{
    public class ComOffersWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ComOfferDto>>
    {
        public ComOfferFilterForParticipant comOfferFilterFor { get; set; }
    }
    public class ComOffersMyWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ComOfferMyDto>>
    {
        public ComOfferFilterForParticipant comOfferFilterFor { get; set; }
    }

    public class ComOffersWithPaginationQueryHandler :
         IRequestHandler<ComOffersWithPaginationQuery, PaginatedData<ComOfferDto>>,
        IRequestHandler<ComOffersMyWithPaginationQuery, PaginatedData<ComOfferMyDto>>
        
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ComOffersWithPaginationQueryHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ComOffersWithPaginationQueryHandler> _logger;
        private readonly IDateTime _dateTime;

        public ComOffersWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
           ILogger<ComOffersWithPaginationQueryHandler> logger,
            IStringLocalizer<ComOffersWithPaginationQueryHandler> localizer,
            IDateTime dateTime
            )
        {
            _dateTime = dateTime;
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<PaginatedData<ComOfferDto>> Handle(ComOffersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ComOffersWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<ComOffer>(request.FilterRules);
            switch (request.comOfferFilterFor)
            {
                case ComOfferFilterForParticipant.Actials:
                    var now = _dateTime.Now;
                    filters = filters.And(c => now >= c.TermBegin && now <= c.TermEnd && c.WinnerId!=null  );
                    break;
                case ComOfferFilterForParticipant.Archives:
                    filters = filters.And(c => _dateTime.Now > c.TermEnd && c.WinnerId!=null);
                    break;
                case ComOfferFilterForParticipant.Waitings:
                    filters = filters.And(s => (s.Status !=ComOfferStatus.WinnerDetermined && s.Status!=ComOfferStatus.Cancelled));
                    break;
                default:
                    break;
            }
            var exitSortProperty = PredicateBuilder.CheckProperty<ComOffer>(request.Sort);
            var data = await _context.ComOffers.Where(filters)
                .Include(c=>c.Direction)
                .Include(c=>c.Winner)
                .Include(u=>u.Manager)
                .OrderByWithCheck(request.Sort, request.Order)
                //.OrderBy($"{request.Sort} {request.Order}")
                .Select(c => new ComOfferDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Status = c.Status,
                    Number = c.Number,
                    DateBegin = c.DateBegin,
                    DateEnd = c.DateEnd,
                    DirectionId = c.DirectionId,
                    Direction = _mapper.Map<DirectionDto>(c.Direction),
                    TermBegin = c.TermBegin == default(DateTime) ? default(DateTime?) : c.TermBegin,
                    TermEnd = c.TermEnd == default(DateTime) ? default(DateTime?) : c.TermEnd,
                    ManagerId = c.ManagerId,
                    Manager = c.Manager,
                    DelayDay = c.DelayDay,
                    IsBankDays = c.IsBankDays,
                    WinnerId = c.WinnerId,
                    Winner = c.Winner != null ? _mapper.Map<ContragentDto>(c.Winner) : null,
                    IsDeliveryInPrice = c.IsDeliveryInPrice,
                    //ComParticipants=_mapper.Map<ICollection<ComParticipantDto>>(c.ComParticipants),
                    DeadlineDate = c.ComStages.OrderBy(o => o.Number).LastOrDefault().DeadlineDate

                })
                
                //.ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
                .PaginatedDataLazySortAsync(request.Page, request.Rows,request.Sort,request.Order);
            return data;
            //.ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
            //var dataDto = _mapper.Map<IEnumerable<ComOfferDto>>(data.rows);
            //return new PaginatedData<ComOfferDto>(dataDto, data.total); ;
        }
        public async Task GetLastStages(int contrId, CancellationToken cancellationToken)
        {
            var result = await _context.ComOffers
              .Include(s => s.ComStages)
              .ThenInclude(p => p.StageParticipants)
              .Include(s => s.StageParticipants)
              .Where(s => s.StageParticipants.Any(p => p.ContragentId == contrId))
              
              .ToListAsync(cancellationToken);
              
            var data = result;
              

        }
        private static string GetStatusStr(StageParticipant stageParticipant)
        {
            if (stageParticipant is null) return "";
            return stageParticipant.Status.ToDescriptionString();
        }
        public async Task<PaginatedData<ComOfferMyDto>> Handle(ComOffersMyWithPaginationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Only contragent commercial offers");
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
                {
                    _logger.LogError("Контрагент не найден!");
                    throw new Exception("Контрагент не найден!");
                }
                ContragentId = contragent.Id;
            }
            var filters = PredicateBuilder.FromFilter<ComOffer>(request.FilterRules);
           
            switch (request.comOfferFilterFor)
            {
                case ComOfferFilterForParticipant.Actials:
                    var now = _dateTime.Now;
                    filters = filters.And(o => o.ComParticipants.Any(c => c.ContragentId == ContragentId));
                    filters = filters.And(c=> now>=c.TermBegin && now<=c.TermEnd && c.WinnerId== ContragentId);
                    break;
                case ComOfferFilterForParticipant.Archives:
                    filters = filters.And(o => o.ComParticipants.Any(c => c.ContragentId == ContragentId));
                    filters = filters.And(c => _dateTime.Now > c.TermEnd && c.WinnerId == ContragentId);
                    break;
                case ComOfferFilterForParticipant.Waitings:
                    filters = filters.And(o => o.ComParticipants.Any(c => c.ContragentId == ContragentId));
                    filters = filters.And(s => s.Status ==ComOfferStatus.Waiting || s.Status==ComOfferStatus.Evaluation || s.Status==ComOfferStatus.WinnerDetermining);
                    filters = filters.And(s =>s.WinnerId==default(int?)
                                    && s.StageParticipants.Any(p=>p.ContragentId==ContragentId
                                    && p.Status!=ParticipantStatus.Cancel && p.Status!=ParticipantStatus.FailureParitipate)); 
                    break;
                default:
                    filters = filters.And(o => o.ComParticipants.Any(c => c.ContragentId == ContragentId));
                    filters = filters.And(s => (short)s.Status > 0); break;
            }
           // await GetLastStages(ContragentId,cancellationToken);
            var data = await _context.ComOffers.Where(filters)
                 .Include(c => c.Direction)
                 .Include(c => c.Winner)
                 .Include(u => u.Manager)
                 .Include(s=>s.ComStages)
                 .OrderByWithCheck(request.Sort, request.Order)
                 .Select(c=>new ComOfferMyDto
                 {
                     Id=c.Id,
                     Name=c.Name,
                     Status=c.Status,
                     Number=c.Number,
                     DateBegin=c.DateBegin,
                     DateEnd=c.DateEnd,
                     DirectionId=c.DirectionId,
                     Direction=_mapper.Map<DirectionDto>(c.Direction),
                     TermBegin=c.TermBegin==default(DateTime)? default(DateTime?): c.TermBegin,
                     TermEnd = c.TermEnd == default(DateTime) ? default(DateTime?) : c.TermEnd,
                     ManagerId=c.ManagerId,
                     Manager=c.Manager,
                     DelayDay=c.DelayDay,
                     IsBankDays=c.IsBankDays,
                     WinnerId=c.WinnerId,
                     Winner=c.Winner!=null? _mapper.Map<ContragentDto>(c.Winner): null,
                     IsDeliveryInPrice=c.IsDeliveryInPrice,
                     //ComParticipants=_mapper.Map<ICollection<ComParticipantDto>>(c.ComParticipants),
                     DeadlineDate=c.ComStages.OrderBy(o=>o.Number).LastOrDefault().DeadlineDate,
                     LastStatusStr= (from st in c.ComStages
                                     join sp in _context.StageParticipants on st.Id equals sp.ComStageId
                                     where sp.ContragentId== ContragentId
                                     orderby st.Number descending
                                     select st)
                              .FirstOrDefault()
                              .StageParticipants
                              .FirstOrDefault(sp => sp.ContragentId == ContragentId).Status.ToDescriptionString()

                 })
                 //.ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
                 .PaginatedDataLazySortAsync(request.Page, request.Rows, request.Sort, request.Order);
                 //.PaginatedDataAsync(request.Page, request.Rows);
            //return data;
            //.ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
            // var dataDto = _mapper.Map<IEnumerable<ComOfferDto>>(data.rows);
            return data; // new PaginatedData<ComOfferDto>(data, data.total); ;

        }
    }
}

