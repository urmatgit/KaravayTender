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

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Queries.Pagination
{
    public class ComOffersWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ComOfferDto>>
    {
       
    }
    public class ComOffersMyWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ComOfferDto>>
    {
        
    }

    public class ComOffersWithPaginationQueryHandler :
         IRequestHandler<ComOffersWithPaginationQuery, PaginatedData<ComOfferDto>>,
        IRequestHandler<ComOffersMyWithPaginationQuery, PaginatedData<ComOfferDto>>
        
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ComOffersWithPaginationQueryHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ComOffersWithPaginationQueryHandler> _logger;

        public ComOffersWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
           ILogger<ComOffersWithPaginationQueryHandler> logger,
            IStringLocalizer<ComOffersWithPaginationQueryHandler> localizer
            )
        {
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
           var data = await _context.ComOffers.Where(filters)
                .Include(c=>c.Direction)
                .Include(c=>c.Winner)
                .Include(u=>u.Manager)
                .OrderBy($"{request.Sort} {request.Order}")
                //.ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            //return data;
            //.ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
            var dataDto = _mapper.Map<IEnumerable<ComOfferDto>>(data.rows);
            return new PaginatedData<ComOfferDto>(dataDto, data.total); ;
        }

        public async Task<PaginatedData<ComOfferDto>> Handle(ComOffersMyWithPaginationQuery request, CancellationToken cancellationToken)
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
            var data = await _context.ComOffers.Where(filters)
                 .Include(c => c.Direction)
                 .Include(c => c.Winner)
                 .Include(u => u.Manager)
                 .Where(o=>o.ComParticipants.Any(c=>c.ContragentId==ContragentId))
                 .OrderBy($"{request.Sort} {request.Order}")
                 //.ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
                 .PaginatedDataAsync(request.Page, request.Rows);
            //return data;
            //.ProjectTo<ComOfferDto>(_mapper.ConfigurationProvider)
            var dataDto = _mapper.Map<IEnumerable<ComOfferDto>>(data.rows);
            return new PaginatedData<ComOfferDto>(dataDto, data.total); ;

        }
    }
}

