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
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Application.Common.Specification;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using Microsoft.EntityFrameworkCore.Internal;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Queries.Pagination
{
    public class ComParticipantsWithPaginationQuery : PaginationRequest, IRequest<PaginatedData<ComParticipantDto>>
    {
        public int ComOfferId { get; set; }
    }
    
    public class ComParticipantsWithPaginationQueryHandler :
         IRequestHandler<ComParticipantsWithPaginationQuery, PaginatedData<ComParticipantDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ComParticipantsWithPaginationQueryHandler> _localizer;

        public ComParticipantsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ComParticipantsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<ComParticipantDto>> Handle(ComParticipantsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ComParticipantsWithPaginationQueryHandler method 
            var filters = PredicateBuilder.FromFilter<ComParticipant>(request.FilterRules).And(c => c.ComOfferId == request.ComOfferId);


            //var data0 = from c in _context.ComParticipants
            //            join sp in _context.StageParticipants.Where(s => s.ComOfferId == request.ComOfferId) on c.ContragentId equals sp.ContragentId into spart
            //            from part in spart.DefaultIfEmpty()
            //            where c.ComOfferId == request.ComOfferId
            //            select new
            //            {
            //                c,
            //                part
            //            };
            var lastComStaget = await _context.ComStages
                                .Where(c => c.ComOfferId == request.ComOfferId)
                                .OrderBy(n => n.Number)
                                .LastOrDefaultAsync(cancellationToken);

            var data0 = _context.ComParticipants
                       .Where(filters)
                       .GroupJoin(_context.StageParticipants
                                .Where(c => c.ComOfferId == request.ComOfferId && lastComStaget!=null? c.ComStageId==lastComStaget.Id : false),
                                 cp => cp.ContragentId,
                                 sp => sp.ContragentId,
                                 (c, s) => new { cp = c, sp = s })
                       .SelectMany(
                             x => x.sp.DefaultIfEmpty(),
                             (c, s) => new ComParticipantDto()
                             {
                                 ComOfferId = c.cp.ComOfferId,
                                 ContragentId = c.cp.ContragentId,
                                 ContragentName = c.cp.Contragent.Name,
                                 Status = s == null ?  Domain.Enums.ParticipantStatus.PriceRequest : s.Status,
                                 StepFailure= s == null || s.Status != Domain.Enums.ParticipantStatus.FailureParitipate  ? null: s.ComStage.Number
                                 
                             }
                        );
                       


            //var data1 = await _context.ComStages
            //            .Include(s => s.StageCompositions)
            //            .Where(c => c.ComOfferId == request.ComOfferId)
            //            .OrderBy(o => o.Number)
            //            .LastOrDefaultAsync(cancellationToken)
            //     ;

            //var data = await _context.ComParticipants
            //        .Where(filters)
            //        .Specify(new FilterByComOfferQuerySpec(request.ComOfferId))
            //        .Include(c => c.Contragent)
            //        .OrderBy($"{request.Sort} {request.Order}")
            //        .PaginatedDataAsync(request.Page, request.Rows)
            //        ;
            //if (data1 != null)
            //{
            //    var dataRes = from d in data.rows
            //                  join p in data1?.StageParticipants on d.ContragentId equals p.ContragentId into s
            //                  from sp in s.DefaultIfEmpty()
            //                  select new ComParticipantDto()
            //                  {
            //                      ContragentId = d.ContragentId,
            //                      ContragentName =d.Contragent.Name,
            //                      ComOfferId = d.ComOfferId,
            //                      Status = sp == null ? Domain.Enums.ParticipantStatus.Waiting : sp.Status,
            //                      StepFailure=sp==null? null: sp.
            //                  };

            //    return new PaginatedData<ComParticipantDto>(dataRes, dataRes.Count());

            //}
            //else
            //{
            //var dataDto = _mapper.Map<IEnumerable<ComParticipantDto>>(data0.rows);
            //.ProjectTo<ComParticipantDto>(_mapper.ConfigurationProvider)
            return new PaginatedData<ComParticipantDto>(data0, data0.Count());
            //}
        }
        public class FilterByComOfferQuerySpec : Specification<ComParticipant>
        {
            public FilterByComOfferQuerySpec(int comOfferId)
            {
                Criteria = p => p.ComOfferId == comOfferId;
                
                
            }


        }
    }
}

