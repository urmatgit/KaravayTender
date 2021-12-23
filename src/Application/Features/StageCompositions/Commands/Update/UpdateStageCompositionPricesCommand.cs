using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Application.Features.Contragents.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Caching;
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using CleanArchitecture.Razor.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Update
{
    public class UpdateStageCompositionPricesCommand :  IRequest<Result>
    { 
        public ComPositionDtoEx[] inputPrices { get; set; }
        
        public string Description { get; set; }
        public int StageId { get; set; }
        public int ComOfferId { get; set; }
    }
    public class UpdateStageCompositionPricesManagerCommand : IRequest<Result>
    {
        public  StageComRequestDto stageComRequest { get; set; }
    }
    public class FailureParitipateStageCompositionCommand : IRequest<Result>
    {
        public int StageId { get; set; }
        public int ComOfferId { get; set; }
        public string Description { get; set; }

    }
    public class UpdateStageCompositionPricesCommandHandler :
        IRequestHandler<UpdateStageCompositionPricesCommand, Result>,
        IRequestHandler<FailureParitipateStageCompositionCommand, Result>,
        IRequestHandler<UpdateStageCompositionPricesManagerCommand,Result>
        
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateStageCompositionPricesCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        public UpdateStageCompositionPricesCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            IMediator mediator,
            IStringLocalizer<UpdateStageCompositionPricesCommandHandler> localizer,
             IMapper mapper
            )
        {
            _userManager = userManager;
            _context = context;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<Result> Handle(UpdateStageCompositionPricesCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UpdateStageCompositionPricesCommandHandler method
            var contragentId = await _mediator.Send(new GetContragentIdByUserIdQuery(),cancellationToken);
            
            if (contragentId.Succeeded && contragentId.Data > 0)
            {
                foreach (var req in request.inputPrices)
                {
                    //StageId = req.StageId;
                    //ComOfferId = req.ComOfferId;
                    var item = await _context.StageCompositions.FindAsync(new object[] { req.StageId, contragentId.Data, req.Id }, cancellationToken);
                    if (item is not null && req.InputPrice!=item.Price)
                    {
                        item.Price = req.InputPrice;
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }
            }
            //dc.ComStageId, dc.ContragentId,dc.ComOfferId
            var stageParticipant = await _context.StageParticipants.FindAsync(new object[] { request.StageId, contragentId.Data, request.ComOfferId },cancellationToken);
            if (stageParticipant is not null && stageParticipant.Status == Domain.Enums.ParticipantStatus.PriceRequest)
            {
                stageParticipant.Status = Domain.Enums.ParticipantStatus.PriceConfirmed;
                await _context.SaveChangesAsync(cancellationToken);
            }
            else Result.Failure(new string[] { "Цены уже установлены или не найдена запись!" });
            //if (item != null)
            //{
            //     item = _mapper.Map(request, item);
            //     await _context.SaveChangesAsync(cancellationToken);
            //}
            return Result.Success();
        }

        public async Task<Result> Handle(FailureParitipateStageCompositionCommand request, CancellationToken cancellationToken)
        {
            var contragentId = await _mediator.Send(new GetContragentIdByUserIdQuery(), cancellationToken);
            //dc.ComStageId, dc.ContragentId,dc.ComOfferId
            var stageParticipant = await _context.StageParticipants.FindAsync(new object[] { request.StageId, contragentId.Data, request.ComOfferId }, cancellationToken);
            if (stageParticipant is not null)
            {
                stageParticipant.Status = Domain.Enums.ParticipantStatus.FailureParitipate;
                stageParticipant.Description = request.Description;
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Result.Success();
        }

        public async Task<Result> Handle(UpdateStageCompositionPricesManagerCommand request, CancellationToken cancellationToken)
        {
            foreach(var contrPrice in request.stageComRequest.ContrPrices)
            {
                var stageCom = await _context.StageCompositions.FindAsync(new object[] { request.stageComRequest.StageId, contrPrice.ContrId, contrPrice.ComPositionId },cancellationToken);
                if (stageCom is not null && stageCom.RequestPrice != contrPrice.RequestPrice)
                    stageCom.RequestPrice = contrPrice.RequestPrice;
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
