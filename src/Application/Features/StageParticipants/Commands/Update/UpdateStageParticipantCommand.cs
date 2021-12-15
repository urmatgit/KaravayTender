using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.StageParticipants.Caching;
using CleanArchitecture.Razor.Application.Features.StageParticipants.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.Update
{
    public class UpdateStageParticipantCommand: StageParticipantDto,IRequest<Result>, IMapFrom<StageParticipant>
    {
         public string CacheKey => StageParticipantCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => StageParticipantCacheTokenSource.ResetCacheToken;
    }

    public class UpdateStageParticipantCommandHandler : IRequestHandler<UpdateStageParticipantCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateStageParticipantCommandHandler> _localizer;
        public UpdateStageParticipantCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateStageParticipantCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateStageParticipantCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateStageParticipantCommandHandler method 
           var item =await _context.StageParticipants.FindAsync( new object[] { request.ComStageId,request.ContragentId,request.ComOfferId }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
