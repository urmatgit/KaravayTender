using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComOffers.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.ComStages.Queries.GetBy;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Caching;
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Update
{
    public class UncheckPriceRequesStageCompositionCommand: IRequest<Result>, IMapFrom<StageComposition>
    {

        public int  ComOfferId { get; set; }
    }

    public class UncheckPriceRequesStageCompositionCommandHandler : IRequestHandler<UncheckPriceRequesStageCompositionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<UncheckPriceRequesStageCompositionCommandHandler> _localizer;
        public UncheckPriceRequesStageCompositionCommandHandler(
            IApplicationDbContext context,
            IMediator mediator,
            IStringLocalizer<UncheckPriceRequesStageCompositionCommandHandler> localizer,
             IMapper mapper
            )
        {
            _mediator = mediator;
            _context = context;
            _localizer = localizer;
            _mapper = mapper;

        }
        public async Task<Result> Handle(UncheckPriceRequesStageCompositionCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UncheckPriceRequesStageCompositionCommandHandler method
            var lastStage = await _mediator.Send(new GetByStageLastQuery { ComOfferId = request.ComOfferId },cancellationToken);
            if (lastStage != null)
            {
                var comOffer =await  _mediator.Send(new GeByIdComOffersQuery { Id = request.ComOfferId },cancellationToken);

                var stCom = lastStage.StageCompositions
                    .Where(s => comOffer.Data.ComParticipants.Any(p => p.ContragentId == s.ContragentId)
                            && comOffer.Data.ComPositions.Any(p => p.Id == s.ComPositionId));
                foreach(StageComposition stageComposition in stCom)
                {
                    stageComposition.RequestPrice = false;
                }
                
                 await _context.SaveChangesAsync(cancellationToken);
                
            }
           return Result.Success();
        }
    }
}
