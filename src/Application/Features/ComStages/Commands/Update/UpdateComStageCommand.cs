using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComStages.Caching;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Commands.Update
{
    public class UpdateComStageCommand: ComStageDto,IRequest<Result>, IMapFrom<ComStage>
    {
         public string CacheKey => ComStageCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => ComStageCacheTokenSource.ResetCacheToken;
    }
    public class UpdateDeadlineComStageCommand :  IRequest<Result>
    {
        public DateTime? DeadLine { get; set; }
        public int Id { get; set; }
    }

    public class UpdateComStageCommandHandler : IRequestHandler<UpdateComStageCommand, Result>, IRequestHandler<UpdateDeadlineComStageCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateComStageCommandHandler> _localizer;
        public UpdateComStageCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateComStageCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateComStageCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateComStageCommandHandler method 
           var item =await _context.ComStages.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }

        public async Task<Result> Handle(UpdateDeadlineComStageCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.ComStages.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item != null && request.DeadLine is not null && item.DeadlineDate!=request.DeadLine)
            {
                if (request.DeadLine < item.DeadlineDate)
                    return Result.Failure(new string[] {"Дату можно менять только в сторону увеличения" });
                item.DeadlineDate = request.DeadLine.Value;
                
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Result.Success();
        }
    }
}
