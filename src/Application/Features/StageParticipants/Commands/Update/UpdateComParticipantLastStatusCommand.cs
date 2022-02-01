using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Caching;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Queries.GetAll;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.Update
{
    public class UpdateComParticipantLastStatusCommand: IRequest<Result>
    {
        public int ComOfferId { get; set; }
        public Domain.Enums.ParticipantStatus ParticipantStatus { get; set; }
         public string CacheKey => ComParticipantCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => ComParticipantCacheTokenSource.ResetCacheToken;
    }

    public class UpdateComParticipantLastStatusCommandHandler : IRequestHandler<UpdateComParticipantLastStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateComParticipantLastStatusCommandHandler> _localizer;
        private readonly IMediator _mediator;
        public UpdateComParticipantLastStatusCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateComParticipantLastStatusCommandHandler> localizer,
             IMapper mapper,
             IMediator mediator
            )
        {
            _mediator = mediator;
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateComParticipantLastStatusCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UpdateComParticipantLastStatusCommandHandler method 
            var comOfferLastStageParticipants = await _mediator.Send(new GetParticipantsLastWithQuery() {ComOfferId=request.ComOfferId },cancellationToken);
            if (comOfferLastStageParticipants.Succeeded)
            {
                foreach(var row in comOfferLastStageParticipants.Data)
                {
                    if (row.Status == Domain.Enums.ParticipantStatus.Confirmed  || row.Status == Domain.Enums.ParticipantStatus.Request)
                    {
                         var stageParticipant = await _context.StageParticipants.FindAsync(
                            new object[] { row.ComStageId,row.ContragentId,row.ComOfferId }, cancellationToken);
                        if (stageParticipant is not null)
                        {
                            stageParticipant.Status = request.ParticipantStatus;
                        }
                    }
                }
               await _context.SaveChangesAsync(cancellationToken);
            }
           return Result.Success();
        }
    }
}
