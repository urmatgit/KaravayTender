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
    public class CreateComStageCommand: ComStageDto,IRequest<Result<ComStageDto>>, IMapFrom<ComStage>
    {
        public string CacheKey => ComStageCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComStageCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateComStageCommandHandler : IRequestHandler<CreateComStageCommand, Result<ComStageDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<CreateComStageCommand> _localizer;
        public CreateComStageCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateComStageCommand> localizer,
            IMapper mapper,
            IMediator mediator 
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<Result<ComStageDto>> Handle(CreateComStageCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing CreateComStageCommandHandler method
            var comoffer = await _context.ComOffers
                   .Include(c => c.ComPositions)
                   .Include(c => c.ComParticipants)
                   .Where(c => c.Id == request.ComOfferId)
                   .FirstOrDefaultAsync(cancellationToken);
            if (comoffer.ComPositions==null || comoffer.ComPositions.Count==0)

                return Result<ComStageDto>.Failure( new string[] { ErrorMessages.PositionsNotFound });
            if (comoffer.ComParticipants == null || comoffer.ComParticipants.Count == 0)

                return Result<ComStageDto>.Failure(new string[] { ErrorMessages.ParcipantsNotFound });

            var item = await _context.ComStages.FirstOrDefaultAsync(c => c.ComOfferId == request.ComOfferId && c.Number == request.Number,cancellationToken);
            if (item != null)
            {
                if (item.DeadlineDate != request.DeadlineDate)
                {
                    item.DeadlineDate = request.DeadlineDate;
                }
            }
            else
            {
                  item = _mapper.Map<ComStage>(request);
                _context.ComStages.Add(item);
            }
           await _context.SaveChangesAsync(cancellationToken);
            var stageComResult = await _mediator.Send(new CreateStageCompositionsCommand() { ComOfferId = request.ComOfferId, ComStageId = item.Id },cancellationToken);
            if (!stageComResult.Succeeded)
            {
                return Result<ComStageDto>.Failure(stageComResult.Errors);
            }
            //Create StageParticipants
            var stageParticipants = await _mediator.Send(new CreateStageParticipantsCommand { comOfferDto = stageComResult.Data, ComStageId = item.Id },cancellationToken);
            if (!stageParticipants.Succeeded)
            {
                return Result<ComStageDto>.Failure(stageParticipants.Errors);
            }
            var itemDto = _mapper.Map<ComStageDto>(item);
           return  Result<ComStageDto>.Success(itemDto);
        }
    }
}
