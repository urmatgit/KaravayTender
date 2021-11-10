using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Directions.Caching;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Directions.Commands.Create
{
    public class CreateDirectionCommand: DirectionDto,IRequest<Result<int>>, IMapFrom<Direction>
    {
        public string CacheKey => DirectionCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => DirectionCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateDirectionCommandHandler : IRequestHandler<CreateDirectionCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateDirectionCommand> _localizer;
        public CreateDirectionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateDirectionCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateDirectionCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing CreateDirectionCommandHandler method 
           var item = _mapper.Map<Direction>(request);
           _context.Directions.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  Result<int>.Success(item.Id);
        }
    }
}
