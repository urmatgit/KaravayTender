using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Application.Features.StageParticipants.Caching;
using CleanArchitecture.Razor.Application.Features.StageParticipants.DTOs;
using CleanArchitecture.Razor.Application.Features.StageParticipants.EventHandlers;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.Create
{
    public class CreateStageParticipantsCommand:IRequest<Result>, IMapFrom<IEnumerable< StageParticipant>>
    {
        public ComOfferDto comOfferDto { get; set; }
        public int ComStageId { get; set; }
        public string CacheKey => StageParticipantCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => StageParticipantCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateStageParticipantsCommandHandler : IRequestHandler<CreateStageParticipantsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateStageParticipantsCommand> _localizer;
        public CreateStageParticipantsCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateStageParticipantsCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateStageParticipantsCommand request, CancellationToken cancellationToken)
        {
           if (request.comOfferDto?.ComParticipants.Count > 0)
            {
                List<StageParticipant> stageParticipants = new List<StageParticipant>();
                foreach(var participant in request.comOfferDto.ComParticipants)
                {
                    var stPart = new StageParticipant
                    {
                        ComOfferId = request.comOfferDto.Id,
                        ComStageId = request.ComStageId,
                        ContragentId = participant.ContragentId,
                        Status = Domain.Enums.ParticipantStatus.Request
                    };
                    //builder.HasKey(dc => new { dc.ComStageId, dc.ContragentId, dc.ComOfferId });
                    var exist = await _context.StageParticipants.FindAsync(new object[] { request.ComStageId, participant.ContragentId, request.comOfferDto.Id },cancellationToken);
                    if (exist==null)
                    {
                        stageParticipants.Add(stPart);
                        var createevent = new StageParticipantUpdatedEvent(stPart);
                        stPart.DomainEvents.Add(createevent);
                    }
                }
                if (stageParticipants.Count > 0)
                {
                    await _context.StageParticipants.AddRangeAsync(stageParticipants, cancellationToken);
                    
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            return await Result.SuccessAsync();
        }
    }
}
