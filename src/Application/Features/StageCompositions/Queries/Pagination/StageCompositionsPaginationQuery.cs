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
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Application.Common.Specification;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Razor.Domain.Identity;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Queries.Pagination
{
    public class StageCompositionsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<StageCompositionDto>>
    {
        public int ComStageId { get; set; }
    }
    public class StageCompositionsWithByContragentPaginationQuery : PaginationRequest, IRequest<PaginatedData<StageCompositionDto>>
    {
        public int ComStageId { get; set; }
        public int ContragentId { get; set; }
    }

    public class StageCompositionsWithPaginationQueryHandler :
         IRequestHandler<StageCompositionsWithPaginationQuery, PaginatedData<StageCompositionDto>>,
        IRequestHandler<StageCompositionsWithByContragentPaginationQuery, PaginatedData<StageCompositionDto>>
        
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<StageCompositionsWithPaginationQueryHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        public StageCompositionsWithPaginationQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IStringLocalizer<StageCompositionsWithPaginationQueryHandler> localizer
            )
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<StageCompositionDto>> Handle(StageCompositionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing StageCompositionsWithPaginationQueryHandler method 
           var filters = PredicateBuilder.FromFilter<StageComposition>(request.FilterRules);
           var data = await _context.StageCompositions
                .Specify(new FilterByComStageQuerySpec(request.ComStageId))
                .Where(filters)
                .Include(s=>s.Contragent)
                .Include(s=>s.ComPosition)
                .OrderBy($"{request.Sort} {request.Order}")
                .ProjectTo<StageCompositionDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }

        public async Task<PaginatedData<StageCompositionDto>> Handle(StageCompositionsWithByContragentPaginationQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (currentUser is  null)
            {
                throw new Exception( "Пользователь не найден!");
            }
            if (currentUser.ContragentId.HasValue)
            {
                request.ContragentId = currentUser.ContragentId.Value;

            }else
            {
                var contragent = await _context.Contragents.FirstOrDefaultAsync(c => c.ApplicationUserId == currentUser.Id, cancellationToken);
                if (contragent is null)
                    throw new Exception("Пользователь не найден!");
                request.ContragentId = contragent.Id;
            }
            var filters = PredicateBuilder.FromFilter<StageComposition>(request.FilterRules);
            var data = await _context.StageCompositions
                 .Specify(new FilterByComStageQuerySpec(request.ComStageId,request.ContragentId))
                 .Where(filters)
                 .Include(s=>s.ComStage)
                 .Include(s => s.Contragent)
                 .Include(s => s.ComPosition)
                 .OrderBy($"{request.Sort} {request.Order}")
                 .ProjectTo<StageCompositionDto>(_mapper.ConfigurationProvider)
                 .PaginatedDataAsync(request.Page, request.Rows);
            return data;
        }

        public class FilterByComStageQuerySpec : Specification<StageComposition>
        {
            public FilterByComStageQuerySpec(int comStageId,int contragentid=0)
            {
                if (contragentid==0)
                    Criteria = p => p.ComStageId == comStageId;
                else
                    Criteria = p => p.ComStageId == comStageId && p.ContragentId==contragentid;
            }


        }
    }
}

