using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComOffers.Caching;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Update
{
    public class UpdateComOfferCommand: ComOfferDto,IRequest<Result>, IMapFrom<ComOffer>
    {
         public string CacheKey => ComOfferCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => ComOfferCacheTokenSource.ResetCacheToken;
    }

    public class UpdateComOfferCommandHandler : IRequestHandler<UpdateComOfferCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateComOfferCommandHandler> _localizer;
        public UpdateComOfferCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateComOfferCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateComOfferCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateComOfferCommandHandler method 
           var item =await _context.ComOffers.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
