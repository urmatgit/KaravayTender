using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Constants;
using CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Update;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Application.Features.ComOffers.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Features.ComStages.Commands.Create;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Update
{
    public class UpdateStageCompositionPricesCommand : IRequest<Result<ComOfferDto>>
    {
        public ComPositionDtoEx[] inputPrices { get; set; }

        public string Description { get; set; }
        public int StageId { get; set; }
        public int ComOfferId { get; set; }
        public Domain.Enums.ParticipantStatus status { get; set; } = Domain.Enums.ParticipantStatus.Confirmed;
    }
    public class UpdateStageCompositionPricesManagerCommand : IRequest<Result>
    {
        public StageComRequestDto stageComRequest { get; set; }
        public DateTime Deadline { get; set; }
    }
    public class FailureParitipateStageCompositionCommand : UpdateStageCompositionPricesCommand
    {
        public FailureParitipateStageCompositionCommand()
        {
            status = Domain.Enums.ParticipantStatus.FailureParitipate;
        }

    }
    public class UpdateStageCompositionPricesCommandHandler :
        IRequestHandler<UpdateStageCompositionPricesCommand, Result<ComOfferDto>>,
        IRequestHandler<FailureParitipateStageCompositionCommand, Result<ComOfferDto>>,
        IRequestHandler<UpdateStageCompositionPricesManagerCommand, Result>

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
        public async Task<Result<ComOfferDto>> Handle(UpdateStageCompositionPricesCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UpdateStageCompositionPricesCommandHandler method
            var contragentId = await _mediator.Send(new GetContragentIdByUserIdQuery(), cancellationToken);
            if (!contragentId.Succeeded || contragentId.Data == 0)
                return Result<ComOfferDto>.Failure(new string[] { Constants.ErrorMessages.NotFoundContragent });

            var comoffer = await _mediator.Send(new GeByIdComOffersQuery() { Id = request.ComOfferId, FullInfo = false }, cancellationToken);
            if (!comoffer.Succeeded)
                Result.Failure(new string[] { Constants.ErrorMessages.NotFoundComOffer });

            switch (comoffer.Data.Status)
            {
                case Domain.Enums.ComOfferStatus.Waiting:
                    foreach (var req in request.inputPrices)
                    {
                        //StageId = req.StageId;
                        //ComOfferId = req.ComOfferId;
                        var item = await _context.StageCompositions.FindAsync(new object[] { req.StageId, contragentId.Data, req.Id }, cancellationToken);
                        if (item is not null && req.InputPrice != item.Price)
                        {
                            item.Price = req.InputPrice;

                            await _context.SaveChangesAsync(cancellationToken);
                        }
                    }

                    //dc.ComStageId, dc.ContragentId,dc.ComOfferId
                    //if (! await UpdateStatusParticipant(request.StageId, contragentId.Data, request.ComOfferId,request,cancellationToken))
                    var stageParticipant = await _context.StageParticipants.FindAsync(new object[] { request.StageId, contragentId.Data, request.ComOfferId }, cancellationToken);
                    if (stageParticipant is not null) // && stageParticipant.Status == Domain.Enums.ParticipantStatus.PriceRequest)
                    {
                        stageParticipant.Status = request.status;
                        stageParticipant.Description = request.Description;
                        await _context.SaveChangesAsync(cancellationToken);
                    }else 
                                Result.Failure(new string[] { Constants.ErrorMessages.NotFoundStageParticipant });
                    break;
                case Domain.Enums.ComOfferStatus.WinnerDetermining:
                    var ItsMe = await _context.CheckPartipiatnIsWinning(request.StageId, contragentId.Data, request.ComOfferId, cancellationToken);
                    if (ItsMe)
                    {
                        if (request.status == Domain.Enums.ParticipantStatus.Confirmed)
                        {
                            var updatecomoffer =await _mediator.Send(new UpdateStatusComOfferCommand() { Id = request.ComOfferId, Status = Domain.Enums.ComOfferStatus.WinnerDetermined, WinnerId = contragentId.Data },cancellationToken);
                            if (!updatecomoffer.Succeeded)
                                return Result<ComOfferDto>.Failure(updatecomoffer.Errors);
                            if (!await UpdateStatusParticipant(request.StageId, contragentId.Data, request.ComOfferId, request, cancellationToken))
                                return     Result<ComOfferDto>.Failure(new string[] { Constants.ErrorMessages.NotFoundStageParticipant });

                        }
                        else 
                        if (request.status == Domain.Enums.ParticipantStatus.FailureParitipate) // если отказал
                        {
                            var stageParticipant1 = await _context.StageParticipants.FindAsync(new object[] { request.StageId, contragentId.Data, request.ComOfferId }, cancellationToken);
                            if (stageParticipant1 is not null) // && stageParticipant.Status == Domain.Enums.ParticipantStatus.PriceRequest)
                            {
                                stageParticipant1.Status = Domain.Enums.ParticipantStatus.FailureParitipate;
                                stageParticipant1.Description = request.Description;
                                
                            }
                            var updatecomoffer = await _mediator.Send(new UpdateStatusComOfferCommand() { Id = request.ComOfferId, Status = Domain.Enums.ComOfferStatus.Evaluation, WinnerId =null }, cancellationToken);
                            if (!updatecomoffer.Succeeded)
                                return Result<ComOfferDto>.Failure(updatecomoffer.Errors);

                            await _context.SaveChangesAsync(cancellationToken);
                        }
                    }
                    break;
            
        }
            
            return Result<ComOfferDto>.Success(comoffer.Data);
        }
     private async Task<bool> UpdateStatusParticipant(int StageId,int contragentId,int ComOfferId, UpdateStageCompositionPricesCommand request, CancellationToken cancellationToken)
        {
            var participants = await _context
                                        .GetParicipantsForLastStage(ComOfferId)
                                        .ToListAsync(cancellationToken);
            if (participants == null || participants.Count == 0)
                return false;
            foreach(var part in participants)
            {
                var stageParticipant = await _context.StageParticipants.FindAsync(new object[] { part.ComStageId,part.ContragentId, ComOfferId }, cancellationToken);
                if (stageParticipant is not null) // && stageParticipant.Status == Domain.Enums.ParticipantStatus.PriceRequest)
                {
                    if (part.ContragentId == contragentId)
                        stageParticipant.Status = request.status;
                    else if (part.Status == Domain.Enums.ParticipantStatus.Confirmed || part.Status == Domain.Enums.ParticipantStatus.Request)
                        stageParticipant.Status = Domain.Enums.ParticipantStatus.Cancel;
                    else continue;
                    stageParticipant.Description = request.Description;
                    

                }
            }
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    public async Task<Result<ComOfferDto>> Handle(FailureParitipateStageCompositionCommand request, CancellationToken cancellationToken)
    {
        UpdateStageCompositionPricesCommand updatePrice = request as UpdateStageCompositionPricesCommand;

        return await Handle(updatePrice, cancellationToken);
        //var contragentId = await _mediator.Send(new GetContragentIdByUserIdQuery(), cancellationToken);
        ////dc.ComStageId, dc.ContragentId,dc.ComOfferId
        //var stageParticipant = await _context.StageParticipants.FindAsync(new object[] { request.StageId, contragentId.Data, request.ComOfferId }, cancellationToken);
        //if (stageParticipant is not null)
        //{
        //    stageParticipant.Status = Domain.Enums.ParticipantStatus.FailureParitipate;
        //    stageParticipant.Description = request.Description;
        //    await _context.SaveChangesAsync(cancellationToken);
        //}
        //return Result.Success();
    }

    public async Task<Result> Handle(UpdateStageCompositionPricesManagerCommand request, CancellationToken cancellationToken)
    {

            if (request.stageComRequest.ContrPrices == null || request.stageComRequest.ContrPrices.Count == 0)
                return Result.Failure(new string[] {Constants.ErrorMessages.ParcipantsNotFound });
        var next = await _mediator.Send(new NextComStageCommand() { ComOfferId = request.stageComRequest.ComOfferId, DeadlineDate = request.Deadline }, cancellationToken);
        if (next is null)
            return Result.Failure(next.Errors);
        List<int> ForChangeParticipantStatus = new List<int>();
        foreach (var contrPrice in request.stageComRequest.ContrPrices)
        {
            //var stageCom = await _context.StageCompositions.FindAsync(new object[] { next.Data.Id, contrPrice.ContrId, contrPrice.ComPositionId }, cancellationToken);
            var stageCom = await _context.StageCompositions
                                                .FindAsync(new object[] { next.Data.Id, contrPrice.ContrId, contrPrice.ComPositionId }, cancellationToken);
            if (stageCom is not null)
            {
                //Обнуляем цены если стоит галочка
                if (contrPrice.RequestPrice)
                {
                    if (!ForChangeParticipantStatus.Contains(contrPrice.ContrId))
                        ForChangeParticipantStatus.Add(contrPrice.ContrId);
                    if ((stageCom.Price != default(decimal?) || stageCom.Price != 0m))
                        stageCom.Price = default(decimal?);
                }
                if (stageCom.RequestPrice != contrPrice.RequestPrice)
                    stageCom.RequestPrice = contrPrice.RequestPrice;
            }
        }
        await _context.SaveChangesAsync(cancellationToken);
        foreach (int contrid in ForChangeParticipantStatus)
        {
            var sp = await _context.StageParticipants.FindAsync(new object[] { next.Data.Id, contrid, request.stageComRequest.ComOfferId }, cancellationToken);
            if (sp is not null)
            {
                sp.Status = Domain.Enums.ParticipantStatus.Request;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        return Result.Success();
    }

}
}
