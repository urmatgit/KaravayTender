using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComOffers.Caching;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.ComPositions.Commands.AddEdit;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.AddEdit
{
    public class CopyComOfferCommand: IRequest<Result<ComOfferDto>>, IMapFrom<ComOffer>
    {
        public int Id { get; set; }
        public string CacheKey => ComOfferCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComOfferCacheTokenSource.ResetCacheToken;
    }

    public class CopyComOfferCommandHandler : IRequestHandler<CopyComOfferCommand, Result<ComOfferDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CopyComOfferCommandHandler> _localizer;
        private readonly IDateTime _dateTime;
        private readonly IMediator _mediator;
        private readonly ILogger<CopyComOfferCommandHandler> _logger;
        public CopyComOfferCommandHandler(
            IApplicationDbContext context,
            IDateTime dateTime,
            IStringLocalizer<CopyComOfferCommandHandler> localizer,
            IMapper mapper,
            IMediator mediator,
            ILogger<CopyComOfferCommandHandler> logger
            )
        {
            _logger = logger;
            _mediator = mediator;
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
            _dateTime = dateTime;
        }
        public async Task<Result<ComOfferDto>> Handle(CopyComOfferCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing CopyComOfferCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.ComOffers.AsNoTracking().FirstOrDefaultAsync(f=>f.Id== request.Id , cancellationToken);
                
                item.Id = 0;
                item.Status = Domain.Enums.ComOfferStatus.Preparation;
                item.TermBegin = default(DateTime);
                item.TermEnd = default(DateTime);
                item.Winner = null;
                item.WinnerId = null;
                item.ComParticipants = null;
                item.ComPositions = null;
                item.DateBegin= _dateTime.Now;
                item.DateEnd = null;

                item.Created = _dateTime.Now;
                item.LastModified = null;
                item.LastModifiedBy = "";
                item.CreatedBy = "";

                _context.ComOffers.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"Commercial offer ({request.Id}) copied to {item.Id}");

                //
                var copiedComPositionCount = await _mediator.Send(new CopyComPositionsCommand { ComOfferId = request.Id,NewComOfferId=item.Id });
                if (copiedComPositionCount.Succeeded)
                {
                    _logger.LogInformation($"Commercial offer ({request.Id}) positions  {copiedComPositionCount.Data} copied to {item.Id} ");
                }
                else
                    _logger.LogInformation($"Commercial offer ({request.Id}) positions  {copiedComPositionCount.Data} is not copied to {item.Id} ");
                //
                var copiedCoPartResult = await _mediator.Send(new CopyComParticipantCommand { ComOfferId = request.Id, NewComOfferId = item.Id });
                if (copiedCoPartResult.Succeeded)
                {
                    _logger.LogInformation($"Commercial offer ({request.Id}) participants  {copiedCoPartResult.Data} copied to {item.Id} ");
                }
                else
                    _logger.LogInformation($"Commercial offer ({request.Id}) participants  {copiedCoPartResult.Data} is not copied to {item.Id} ");

                var itemDto = _mapper.Map<ComOfferDto>(item);
                return Result<ComOfferDto>.Success(itemDto);
            }
            else
            {
             
                return Result<ComOfferDto>.Failure(new string[] {"Не выбран контрагент для копирование" });
            }
           
        }
    }
}
