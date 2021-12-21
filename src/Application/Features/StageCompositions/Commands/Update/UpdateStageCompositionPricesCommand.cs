using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Caching;
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
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
        public UpdateStageCompositionPricesCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateStageCompositionPricesCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateStageCompositionPricesCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateStageCompositionPricesCommandHandler method 
           //var item =await _context.StageCompositions.FindAsync( new object[] { request.ComStageId, request.ContragentId, request.ComPositionId }, cancellationToken);
           //if (item != null)
           //{
           //     item = _mapper.Map(request, item);
           //     await _context.SaveChangesAsync(cancellationToken);
           //}
           return Result.Success();
        }
    }
}
