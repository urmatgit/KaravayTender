using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Constants;
using CleanArchitecture.Razor.Application.Features.ComStages.Caching;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Create;
using CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.Create;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Commands.Create
{
    public class NextComStageCommand : ComStageDto, IRequest<Result<ComStageDto>>, IMapFrom<ComStage>
    {
        public string CacheKey => ComStageCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComStageCacheTokenSource.ResetCacheToken;
    }

    public class NextComStageCommandHandler : IRequestHandler<NextComStageCommand, Result<ComStageDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<NextComStageCommand> _localizer;
        public NextComStageCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<NextComStageCommand> localizer,
            IMapper mapper,
            IMediator mediator
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<Result<ComStageDto>> Handle(NextComStageCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing NextComStageCommandHandler method


            var last = await _context.ComStages
                .Include(l => l.StageCompositions)
                .Include(l => l.StageParticipants)
                .Include(c => c.ComOffer)
                .Where(c => c.ComOfferId == request.ComOfferId).OrderBy(o => o.Number)
                .AsNoTracking()
                .LastOrDefaultAsync(cancellationToken);
            if (last is null)
            {
                return Result<ComStageDto>.Failure(new string[] { ErrorMessages.NotFoundComStage });
            }
            if (last.ComOffer.Status != CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Evaluation)
            {
                return Result<ComStageDto>.Failure(new string[] { ErrorMessages.StatusDontMatchForSentRequest });
            }

            last.Number = last.Number + 1;
            last.DeadlineDate = request.DeadlineDate;
            last.ComOffer = null;
            last.Id = 0;
            var ExcludedParticipants = last.StageParticipants.Where(p => p.Status == Domain.Enums.ParticipantStatus.Excluded || p.Status == Domain.Enums.ParticipantStatus.FailureParitipate);
            if (ExcludedParticipants != null && ExcludedParticipants.Any())
                foreach (var part in ExcludedParticipants)
                    last.StageParticipants.Remove(part);
            foreach (var sc in last.StageCompositions)
                sc.ComStageId = 0;
            foreach (var sc in last.StageParticipants)
            {
                sc.ComStageId = 0;
                sc.Status = Domain.Enums.ParticipantStatus.PriceRequest;
            }

            _context.ComStages.Add(last);
            await _context.SaveChangesAsync(cancellationToken);
            




            //var stageComResult = await _mediator.Send(new CreateStageCompositionsCommand() { ComOfferId = request.ComOfferId, ComStageId = nextComStage.Id }, cancellationToken);
            //if (!stageComResult.Succeeded)
            //{
            //    return Result<ComStageDto>.Failure(stageComResult.Errors);
            //}
            ////Create StageParticipants
            //var stageParticipants = await _mediator.Send(new CreateStageParticipantsCommand { comOfferDto = stageComResult.Data, ComStageId = nextComStage.Id }, cancellationToken);
            //if (!stageParticipants.Succeeded)
            //{
            //    return Result<ComStageDto>.Failure(stageParticipants.Errors);
            //}
            var itemDto = _mapper.Map<ComStageDto>(last);
            return Result<ComStageDto>.Success(itemDto);
        }
    }
}
