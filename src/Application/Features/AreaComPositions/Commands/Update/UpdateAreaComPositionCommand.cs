using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.AreaComPositions.Caching;
using CleanArchitecture.Razor.Application.Features.AreaComPositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.AreaComPositions.Commands.Update
{
    public class UpdateAreaComPositionCommand: AreaComPositionDto,IRequest<Result>, IMapFrom<AreaComPosition>
    {
         public string CacheKey => AreaComPositionCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => AreaComPositionCacheTokenSource.ResetCacheToken;
    }

    public class UpdateAreaComPositionCommandHandler : IRequestHandler<UpdateAreaComPositionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateAreaComPositionCommandHandler> _localizer;
        public UpdateAreaComPositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateAreaComPositionCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateAreaComPositionCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateAreaComPositionCommandHandler method 
           var item =await _context.AreaComPositions.FindAsync( new object[] { request.AreaId,request.ComPositionId }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
