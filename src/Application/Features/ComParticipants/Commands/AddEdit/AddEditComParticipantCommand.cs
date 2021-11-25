using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Caching;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.AddEdit
{
    public class AddEditComParticipantCommand: ComParticipantDto,IRequest<Result<int,int>>, IMapFrom<ComParticipant>
    {
        public string CacheKey => ComParticipantCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComParticipantCacheTokenSource.ResetCacheToken;
    }

    public class AddEditComParticipantCommandHandler : IRequestHandler<AddEditComParticipantCommand, Result<int,int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditComParticipantCommandHandler> _localizer;
        public AddEditComParticipantCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditComParticipantCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int,int>> Handle(AddEditComParticipantCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditComParticipantCommandHandler method 
            if (request.ContragentId > 0 && request.ComOfferId>0)
            {
                var item = await _context.ComParticipants.FindAsync(new object[] { request.ContragentId,request.ComOfferId }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int,int>.Success(item.ContragentId,item.ComOfferId);
            }
            else
            {
                var item = _mapper.Map<ComParticipant>(request);
                _context.ComParticipants.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int,int>.Success(item.ContragentId,item.ComOfferId);
            }
           
        }
    }
}
