using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Constants;
using CleanArchitecture.Razor.Application.Features.ComStages.Caching;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Application.Features.ComStages.EventHandlers;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Create;
using CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.Create;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Commands.Create
{
    public class NextComStageCommand : ComStageDto, IRequest<Result<ComStageDto>>, IMapFrom<ComStage>
    {
        public string CacheKey => ComStageCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComStageCacheTokenSource.ResetCacheToken;
    }
    public class NextComStageWinnerCommand : IRequest<Result<ComStageDto>>, IMapFrom<ComStage>
    {
        public int ContragentId { get; set; }
        public int ComOfferId { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string Message { get; set; }
    }
    public class NextComStageCommandHandler : IRequestHandler<NextComStageCommand, Result<ComStageDto>>,
        IRequestHandler<NextComStageWinnerCommand, Result<ComStageDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<NextComStageCommand> _localizer;
        private readonly ILogger<NextComStageCommand> _logger;
        public NextComStageCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<NextComStageCommand> localizer,
            IMapper mapper,
            IMediator mediator,
            ILogger<NextComStageCommand> logger
            )
        {
            _logger = logger;
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<Result<ComStageDto>> Handle(NextComStageCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing NextComStageCommandHandler method
            var lastComStage = _context.GetParicipantsForLastStageWithInfo(request.ComOfferId);
            var datas = await lastComStage
                      .ToListAsync(cancellationToken);
            var maxNumber = datas.Max(x => x.Number);
            var last = new ComStage();
            last.ComOfferId = request.ComOfferId;
            last.Number = maxNumber + 1;
            last.DeadlineDate = request.DeadlineDate;
            last.StageCompositions = new List<StageComposition>();
            foreach (var pos in datas.Where(x => x.Status == Domain.Enums.ParticipantStatus.Confirmed))// .GroupBy(g=>new {g.ContragentId,g.ComPositionId }))
            {
                last.StageCompositions.Add(new  StageComposition
                {
                    ComPositionId = pos.ComPositionId,
                    ContragentId = pos.ContragentId,
                    Price = pos.Price,
                    RequestPrice = pos.RequestPrice,
                    

                });
            }
            last.StageParticipants = new List<StageParticipant>();
            foreach (var pos in datas.Where(x => x.Status == Domain.Enums.ParticipantStatus.Confirmed).GroupBy(g =>  g.ContragentId  ))
            {
                last.StageParticipants.Add(new StageParticipant
                {
                    ContragentId = pos.Key,
                    ComOfferId = request.ComOfferId,
                    Status = Domain.Enums.ParticipantStatus.Confirmed
                });
            }
            _context.ComStages.Add(last);
            await _context.SaveChangesAsync(cancellationToken);
            //var last = await _context.ComStages
            //    .Include(l => l.StageCompositions)
            //    .Include(l => l.StageParticipants)
            //    .Include(c => c.ComOffer)
            //    .Where(c => c.ComOfferId == request.ComOfferId).OrderBy(o => o.Number)
            //    .AsNoTracking()
            //    .LastOrDefaultAsync(cancellationToken);
            //if (last is null)
            //{
            //    return Result<ComStageDto>.Failure(new string[] { ErrorMessages.NotFoundComStage });
            //}
            //if (last.ComOffer.Status != CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Evaluation)
            //{
            //    return Result<ComStageDto>.Failure(new string[] { ErrorMessages.StatusDontMatchForSentRequest });
            //}

            //last.Number = last.Number + 1;
            //last.DeadlineDate = request.DeadlineDate;
            //last.ComOffer = null;
            //last.Id = 0;
            //var ExcludedParticipants = last.StageParticipants.Where(p => p.Status == Domain.Enums.ParticipantStatus.Cancel || p.Status == Domain.Enums.ParticipantStatus.FailureParitipate);
            //if (ExcludedParticipants != null && ExcludedParticipants.Any())
            //    foreach (var part in ExcludedParticipants)
            //        last.StageParticipants.Remove(part);
            //var excludeStCom = last.StageCompositions.Where(s => !last.StageParticipants.Any(p => p.ContragentId == s.ContragentId));
            //foreach (var stcom in excludeStCom)
            //{
            //    stcom.Price = null;
            //    stcom.RequestPrice = false;
            //    //TODO на будущее удалить позиции при отказе поставщика когда переходим на сл. этап
            //    // last.StageCompositions.Remove(stcom);
            //}

            //foreach (var sc in last.StageCompositions)
            //    sc.ComStageId = 0;
            //foreach (var sc in last.StageParticipants)
            //{
            //    sc.ComStageId = 0;
            //    //sc.Status = Domain.Enums.ParticipantStatus.PriceRequest;
            //}

            //_context.ComStages.Add(last);
            //await _context.SaveChangesAsync(cancellationToken);





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

        public async Task<Result<ComStageDto>> Handle(NextComStageWinnerCommand request, CancellationToken cancellationToken)
        {
            var datas = _context.GetParicipantsForLastStage(request.ComOfferId, contragentId: request.ContragentId);
            
            var lastList = await _context.GetFullInfoForCrossData(datas)
                        .OrderByDescending(o => o.Number)
                        .ToListAsync(cancellationToken);
            var last = lastList.FirstOrDefault();
            if (last != null)
            {
                ComStage comStage = new ComStage();
                comStage.Number = last.Number + 1;
                comStage.ComOfferId = request.ComOfferId;
                comStage.DeadlineDate = request.DeadlineDate;
                comStage.StageParticipants = new List<StageParticipant>(){
                    new StageParticipant()
                        {
                            ContragentId = request.ContragentId,
                            ComOfferId = request.ComOfferId,
                            Status=Domain.Enums.ParticipantStatus.Request,
                            
                        }
                };
                comStage.StageCompositions = new List<StageComposition>();
                lastList.ForEach(x =>
                {
                    comStage.StageCompositions.Add(new StageComposition
                    {
                        ComPositionId=x.ComPositionId,
                        ContragentId=x.ContragentId,
                        Price=x.Price,
                        RequestPrice=x.RequestPrice
                    });
                });
                _context.ComStages.Add(comStage);

                var updateEvent = new ComStageUpdatedEvent(comStage);
                comStage.DomainEvents.Add(updateEvent);

                await _context.SaveChangesAsync(cancellationToken);
                var itemDto = _mapper.Map<ComStageDto>(comStage);
                return Result<ComStageDto>.Success(itemDto);
            }
            _logger.LogError("Не найден участник!", request);
            return Result<ComStageDto>.Failure(new string[] { "Не найден участник!" });
        }
    }
}
