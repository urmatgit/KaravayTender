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
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Update
{
    public class UpdateStatusComOfferCommand: IRequest<Result<int>>, IMapFrom<ComOffer>
    {
         public ComOfferStatus Status { get; set; }
        public int Id { get; set; }
         public string CacheKey => ComOfferCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => ComOfferCacheTokenSource.ResetCacheToken;

    }

    public class UpdateStatusComOfferCommandHandler : IRequestHandler<UpdateStatusComOfferCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateStatusComOfferCommandHandler> _localizer;
        public UpdateStatusComOfferCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateStatusComOfferCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(UpdateStatusComOfferCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateStatusComOfferCommandHandler method 
           var item =await _context.ComOffers.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null && item.Status!=request.Status)
           {
                item.Status = request.Status;
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result<int>.Success(request.Id);
        }
    }
}
