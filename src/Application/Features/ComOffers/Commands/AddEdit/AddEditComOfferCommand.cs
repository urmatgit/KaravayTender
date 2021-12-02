using System;
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

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.AddEdit
{
    public class AddEditComOfferCommand: ComOfferDto,IRequest<Result<int>>, IMapFrom<ComOffer>
    {
        public string CacheKey => ComOfferCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComOfferCacheTokenSource.ResetCacheToken;
    }

    public class AddEditComOfferCommandHandler : IRequestHandler<AddEditComOfferCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditComOfferCommandHandler> _localizer;
        private readonly IDateTime _dateTime;
        public AddEditComOfferCommandHandler(
            IApplicationDbContext context,
            IDateTime dateTime,
            IStringLocalizer<AddEditComOfferCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _dateTime = dateTime;
        }
        public async Task<Result<int>> Handle(AddEditComOfferCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditComOfferCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.ComOffers.FindAsync(new object[] { request.Id }, cancellationToken);
                if (request.Status==Domain.Enums.ComOfferStatus.Cancelled && item.DateEnd == default(DateTime))
                {
                    request.DateEnd = _dateTime.Now;
                }
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                request.DateBegin = _dateTime.Now;
                var item = _mapper.Map<ComOffer>(request);
                _context.ComOffers.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }
}