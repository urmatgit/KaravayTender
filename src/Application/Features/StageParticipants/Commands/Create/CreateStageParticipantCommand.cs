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

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.Create
{
    public class CreateStageParticipantCommand: StageParticipantDto,IRequest<Result<StageParticipantDto>>, IMapFrom<StageParticipant>
    {
        public string CacheKey => StageParticipantCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => StageParticipantCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateStageParticipantCommandHandler : IRequestHandler<CreateStageParticipantCommand, Result<StageParticipantDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateStageParticipantCommand> _localizer;
        public CreateStageParticipantCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateStageParticipantCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<StageParticipantDto>> Handle(CreateStageParticipantCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing CreateStageParticipantCommandHandler method 
           var item = _mapper.Map<StageParticipant>(request);
           _context.StageParticipants.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
            var dto = _mapper.Map<StageParticipantDto>(item);
            return Result<StageParticipantDto>.Success(dto);
        }
    }
}
