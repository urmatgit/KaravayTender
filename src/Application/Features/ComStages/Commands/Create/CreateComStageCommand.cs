using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComStages.Caching;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Commands.Create
{
    public class CreateComStageCommand: ComStageDto,IRequest<Result<int>>, IMapFrom<ComStage>
    {
        public string CacheKey => ComStageCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComStageCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateComStageCommandHandler : IRequestHandler<CreateComStageCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateComStageCommand> _localizer;
        public CreateComStageCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateComStageCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateComStageCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing CreateComStageCommandHandler method 
           var item = _mapper.Map<ComStage>(request);
           _context.ComStages.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  Result<int>.Success(item.Id);
        }
    }
}
