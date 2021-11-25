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

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.Update
{
    public class UpdateComParticipantCommand: ComParticipantDto,IRequest<Result>, IMapFrom<ComParticipant>
    {
         public string CacheKey => ComParticipantCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => ComParticipantCacheTokenSource.ResetCacheToken;
    }

    public class UpdateComParticipantCommandHandler : IRequestHandler<UpdateComParticipantCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateComParticipantCommandHandler> _localizer;
        public UpdateComParticipantCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateComParticipantCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateComParticipantCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateComParticipantCommandHandler method 
           var item =await _context.ComParticipants.FindAsync( new object[] { request.ContragentId,request.ComOfferId }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
