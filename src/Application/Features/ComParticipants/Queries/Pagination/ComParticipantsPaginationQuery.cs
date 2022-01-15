using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using System.Linq.Dynamic.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Application.Models;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Application.Common.Specification;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

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
        //TODO make one collection for this functions
        private static   int? getLastStage(IApplicationDbContext _context, int contrId,int comOfferid, StageParticipant s)
        {

            var result =  _context.StageParticipants
                .Include(s => s.ComStage)
                .Where(s => s.ComOfferId == comOfferid && s.ContragentId == contrId)
                .OrderBy(o => o.ComStage.Number)
                .LastOrDefault();
            if (result is not null)
                return result.ComStage.Number;
            return null;
        }
        private static string getLastStageComment(IApplicationDbContext _context, int contrId, int comOfferid, StageParticipant s)
        {

            var result = _context.StageParticipants
                .Include(s => s.ComStage)
                .Where(s => s.ComOfferId == comOfferid && s.ContragentId == contrId)
                .OrderBy(o => o.ComStage.Number)
                .LastOrDefault();
            if (result is not null)
                return result.Description;
            return "";
        }
        public async Task<PaginatedData<ComParticipantDto>> Handle(ComParticipantsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var comOfferId = request.ComOfferId;
            //var res1 = _context.ExecuteSqlRawExt1<ComParticipant, ComParticipant>("select ComOfferId, ContragentId from ComParticipants ", x=>new ComParticipant() {ComOfferId =(int)(long)x[0],ContragentId=(int)(long)x[1]});
            //List<SqlParameter> sqlParameters = new List<SqlParameter>();
            //sqlParameters.Add(new SqlParameter("@comOfferId", comOfferId));
            //var dataraw = _context.ExecuteSqlRawExt1<ComParticipant, SqlParameter>("with s_1 as ( select a.ComOfferId , b.ContragentId  , MAX(a.Number) as Number " +
            //                           "from ComStages a" +
            //                           "  join StageParticipants b on b.ComStageId = a.Id " +
            //                           $" where a.ComOfferId = {comOfferId} " +
            //                           "  group by a.ComOfferId" +
            //                           "       , b.ContragentId" +
            //                           "), s_2 as (" +
            //                           "select a.*, b.Id " +
            //                           "from s_1 a " +
            //                            "join ComStages b on b.ComOfferId = a.ComOfferId " +
            //                            "and b.Number = a.Number " +
            //                           ") " +
            //                        "select a.* " +
            //                        ", b.Status " +
            //                        ", b.Description " +
            //                        "from s_2 a " +
            //                        "join StageParticipants b on b.ComStageId = a.Id " +
            //                        "and b.ContragentId = a.ContragentId",x=>new ComParticipant {
            //                            ComOfferId=(int)(long)x[0],ContragentId=(int)(long)x[1]
            //                        }
            //                        );




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
                                c.Id
                            };

            var dataStep3 = from a in dataStep2
                            join b in _context.StageParticipants on new { Id = a.Id, ContrId = a.a.ContragentId } equals new { Id = b.ComStageId, ContrId = b.ContragentId }
                            join c in _context.Contragents on a.a.ContragentId equals c.Id into contr
                            from d in contr.DefaultIfEmpty()
                            orderby d.Name
                            select new ComParticipantDto
                            {
                                ComOfferId=a.a.ComOfferId,
                                ContragentId=a.a.ContragentId,
                                ContragentName=d.Name,
                                Status= b.Status,
                                StepFailure=b.Status==Domain.Enums.ParticipantStatus.FailureParitipate? a.a.Number : null,
                                Description= b.Description
                            };

             

            var filters = PredicateBuilder.FromFilter<ComParticipantDto>(request.FilterRules,new List<string>() { "StatusStr"});
            var res = await dataStep3
                 .Where(filters)
                .PaginatedDataLazySortAsync(request.Page, request.Rows, request.Sort, request.Order);
            Debug.WriteLine(res.total);
            return res;
            //var dataTemp= _context.ComStages
            //    .GroupJoin(_context.StageParticipants
            //    ,ComStages=>ComStages.Id
            //    ,StageParts=>StageParts.ComStageId
            //    ,(cs,sp)=>new
            //    {
            //        ComOfferId=cs.ComOfferId,
            //        ContragentId=
            //    }
                 //ComOfferId = c.cp.ComOfferId,
                 //                ContragentId = c.cp.ContragentId,
                 //                ContragentName = c.cp.Contragent.Name,
                 //                Status = s == null ? Domain.Enums.ParticipantStatus.PriceRequest : s.Status,
                 //                StepFailure = s == null || s.Status == Domain.Enums.ParticipantStatus.FailureParitipate ? getLastStage(_context, c.cp.ContragentId, c.cp.ComOfferId, s) : null//s.ComStage.Number
                 //                ,
                 //                Description = s == null || s.Status == Domain.Enums.ParticipantStatus.FailureParitipate ? getLastStageComment(_context, c.cp.ContragentId, c.cp.ComOfferId, s) : ""



        }
        //public async Task<PaginatedData<ComParticipantDto>> Handle(ComParticipantsWithPaginationQuery request, CancellationToken cancellationToken)
        //{
        //    await NewQuesry(request.ComOfferId,cancellationToken);
        //    //TODO:Implementing ComParticipantsWithPaginationQueryHandler method 
        //    var filters = PredicateBuilder.FromFilter<ComParticipant>(request.FilterRules).And(c => c.ComOfferId == request.ComOfferId);

           
        //    //var data0 = from c in _context.ComParticipants
        //    //            join sp in _context.StageParticipants.Where(s => s.ComOfferId == request.ComOfferId) on c.ContragentId equals sp.ContragentId into spart
        //    //            from part in spart.DefaultIfEmpty()
        //    //            where c.ComOfferId == request.ComOfferId
        //    //            select new
        //    //            {
        //    //                c,
        //    //                part
        //    //            };
        //    var lastComStaget = await _context.ComStages
        //                        .Where(c => c.ComOfferId == request.ComOfferId)
        //                        .OrderBy(n => n.Number)
        //                        .LastOrDefaultAsync(cancellationToken);

        //    var data0 =await  _context.ComParticipants
        //               .Where(filters)
        //              .OrderByWithCheck(request.Sort, request.Order)
        //               .GroupJoin(_context.StageParticipants
        //                        .Where(c => c.ComOfferId == request.ComOfferId && lastComStaget != null ? c.ComStageId == lastComStaget.Id : false),
        //                         cp => cp.ContragentId,
        //                         sp => sp.ContragentId,
        //                         (c, s) => new { cp = c, sp = s })
        //               .SelectMany(
        //                     x => x.sp.DefaultIfEmpty(),

        //                     (c, s) => new ComParticipantDto()
        //                     {
        //                         ComOfferId = c.cp.ComOfferId,
        //                         ContragentId = c.cp.ContragentId,
        //                         ContragentName = c.cp.Contragent.Name,
        //                         Status = s == null ? Domain.Enums.ParticipantStatus.PriceRequest : s.Status,
        //                         StepFailure = s == null || s.Status == Domain.Enums.ParticipantStatus.FailureParitipate ? getLastStage(_context, c.cp.ContragentId, c.cp.ComOfferId, s) : null//s.ComStage.Number
        //                         ,
        //                         Description = s == null || s.Status == Domain.Enums.ParticipantStatus.FailureParitipate ? getLastStageComment(_context, c.cp.ContragentId, c.cp.ComOfferId, s) : ""

        //                     }
        //                ).PaginatedDataLazySortAsync(request.Page, request.Rows, request.Sort, request.Order);
                       

        //    return data0;

        //    //var data1 = await _context.ComStages
        //    //            .Include(s => s.StageCompositions)
        //    //            .Where(c => c.ComOfferId == request.ComOfferId)
        //    //            .OrderBy(o => o.Number)
        //    //            .LastOrDefaultAsync(cancellationToken)
        //    //     ;

        //    //var data = await _context.ComParticipants
        //    //        .Where(filters)
        //    //        .Specify(new FilterByComOfferQuerySpec(request.ComOfferId))
        //    //        .Include(c => c.Contragent)
        //    //        .OrderBy($"{request.Sort} {request.Order}")
        //    //        .PaginatedDataAsync(request.Page, request.Rows)
        //    //        ;
        //    //if (data1 != null)
        //    //{
        //    //    var dataRes = from d in data.rows
        //    //                  join p in data1?.StageParticipants on d.ContragentId equals p.ContragentId into s
        //    //                  from sp in s.DefaultIfEmpty()
        //    //                  select new ComParticipantDto()
        //    //                  {
        //    //                      ContragentId = d.ContragentId,
        //    //                      ContragentName =d.Contragent.Name,
        //    //                      ComOfferId = d.ComOfferId,
        //    //                      Status = sp == null ? Domain.Enums.ParticipantStatus.Waiting : sp.Status,
        //    //                      StepFailure=sp==null? null: sp.
        //    //                  };

        //    //    return new PaginatedData<ComParticipantDto>(dataRes, dataRes.Count());

        //    //}
        //    //else
        //    //{
        //    //var dataDto = _mapper.Map<IEnumerable<ComParticipantDto>>(data0.rows);
        //    //.ProjectTo<ComParticipantDto>(_mapper.ConfigurationProvider)
        //    //return new PaginatedData<ComParticipantDto>(data0, data0.Count());
        //    //}
        //}
        public class FilterByComOfferQuerySpec : Specification<ComParticipant>
        {
            public FilterByComOfferQuerySpec(int comOfferId)
            {
                Criteria = p => p.ComOfferId == comOfferId;
                
                
            }


        }
    }
}

