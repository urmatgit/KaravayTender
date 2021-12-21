using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
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
          
    }

    public class UpdateStageCompositionPricesCommandHandler : IRequestHandler<UpdateStageCompositionPricesCommand, Result>
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
                    var item = await _context.StageCompositions.FindAsync(new object[] { req.StageId, contragentId.Data, req.Id }, cancellationToken);
                    if (item is not null && req.InputPrice!=item.Price)
                    {
                        item.Price = req.InputPrice;
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }
            }
           //if (item != null)
           //{
           //     item = _mapper.Map(request, item);
           //     await _context.SaveChangesAsync(cancellationToken);
           //}
           return Result.Success();
        }
    }
}
