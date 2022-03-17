using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.StageParticipants.Caching;
using CleanArchitecture.Razor.Application.Features.StageParticipants.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.AddEdit
{
    public class AddEditStageParticipantCommand: StageParticipantDto,IRequest<Result<StageParticipantDto>>, IMapFrom<StageParticipant>
    {
        public string CacheKey => StageParticipantCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => StageParticipantCacheTokenSource.ResetCacheToken;
    }

    public class AddEditStageParticipantCommandHandler : IRequestHandler<AddEditStageParticipantCommand, Result<StageParticipantDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditStageParticipantCommandHandler> _localizer;
        public AddEditStageParticipantCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditStageParticipantCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<StageParticipantDto>> Handle(AddEditStageParticipantCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditStageParticipantCommandHandler method 
            if (request.ComStageId > 0 && request.ContragentId>0 && request.ComOfferId>0)
            {
                var item = await _context.StageParticipants.FindAsync(new object[] { request.ComStageId, request.ContragentId, request.ComOfferId }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                var dto = _mapper.Map<StageParticipantDto>(item);
                return Result<StageParticipantDto>.Success(dto);
            }
            else
            {
                var item = _mapper.Map<StageParticipant>(request);
                _context.StageParticipants.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                var dto = _mapper.Map<StageParticipantDto>(item);
                return Result<StageParticipantDto>.Success(dto);
            }
           
        }
    }
}
