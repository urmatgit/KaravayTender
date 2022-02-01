using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Caching;
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Create
{
    public class CreateStageCompositionCommand: StageCompositionDto,IRequest<Result<StageCompositionDto>>, IMapFrom<StageComposition>
    {
        public string CacheKey => StageCompositionCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => StageCompositionCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateStageCompositionCommandHandler : IRequestHandler<CreateStageCompositionCommand, Result<StageCompositionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateStageCompositionCommand> _localizer;
        public CreateStageCompositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateStageCompositionCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<StageCompositionDto>> Handle(CreateStageCompositionCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing CreateStageCompositionCommandHandler method 
           var item = _mapper.Map<StageComposition>(request);
           _context.StageCompositions.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  Result<StageCompositionDto>.Success(_mapper.Map< StageCompositionDto>(item));
        }
    }
}
