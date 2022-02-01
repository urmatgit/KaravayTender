using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComOffers.Caching;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Delete
{
    public class DeleteComOfferCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => ComOfferCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => ComOfferCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedComOffersCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => ComOfferCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => ComOfferCacheTokenSource.ResetCacheToken;
    }

    public class DeleteComOfferCommandHandler : 
                 IRequestHandler<DeleteComOfferCommand, Result>,
                 IRequestHandler<DeleteCheckedComOffersCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteComOfferCommandHandler> _localizer;
        public DeleteComOfferCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteComOfferCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteComOfferCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteComOfferCommandHandler method 
           var item = await _context.ComOffers.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.ComOffers.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedComOffersCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedComOffersCommandHandler method 
           var items = await _context.ComOffers.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.ComOffers.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
