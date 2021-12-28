using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComOffers.Caching;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Update;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Update
{
    public class UpdateStatusComOfferCommand: IRequest<Result<ComOfferDto>>, IMapFrom<ComOffer>
    {
         public ComOfferStatus Status { get; set; }
        public int Id { get; set; }
         public string CacheKey => ComOfferCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => ComOfferCacheTokenSource.ResetCacheToken;

    }

    public class UpdateStatusComOfferCommandHandler : IRequestHandler<UpdateStatusComOfferCommand, Result<ComOfferDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<UpdateStatusComOfferCommandHandler> _localizer;
        public UpdateStatusComOfferCommandHandler(
            IApplicationDbContext context,
            IMediator mediator,
        IStringLocalizer<UpdateStatusComOfferCommandHandler> localizer,
             IMapper mapper
            )
        {
            _mediator = mediator;
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<ComOfferDto>> Handle(UpdateStatusComOfferCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateStatusComOfferCommandHandler method 
           var item =await _context.ComOffers.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null && item.Status!=request.Status)
           {
                item.Status = request.Status;
                var createevent = new ComOfferUpdatedEvent(item);
                item.DomainEvents.Add(createevent);
                _context.ComOffers.Update(item);
                await _context.SaveChangesAsync(cancellationToken);
                

           }
            var itemDto = _mapper.Map<ComOfferDto>(item);
                 

           return Result<ComOfferDto>.Success(itemDto);
        }
    }
}
