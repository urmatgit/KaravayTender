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

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Create
{
    public class CreateComOfferCommand: ComOfferDto,IRequest<Result<int>>, IMapFrom<ComOffer>
    {
        public string CacheKey => ComOfferCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComOfferCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateComOfferCommandHandler : IRequestHandler<CreateComOfferCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateComOfferCommand> _localizer;
        public CreateComOfferCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateComOfferCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateComOfferCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing CreateComOfferCommandHandler method 
           var item = _mapper.Map<ComOffer>(request);
           _context.ComOffers.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  Result<int>.Success(item.Id);
        }
    }
}
