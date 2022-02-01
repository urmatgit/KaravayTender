using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.AreaComPositions.Caching;
using CleanArchitecture.Razor.Application.Features.AreaComPositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.AreaComPositions.Commands.Create
{
    public class CreateAreaComPositionCommand: AreaComPositionDto,IRequest<Result<int, int>>, IMapFrom<AreaComPosition>
    {
        public string CacheKey => AreaComPositionCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => AreaComPositionCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateAreaComPositionCommandHandler : IRequestHandler<CreateAreaComPositionCommand, Result<int, int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateAreaComPositionCommand> _localizer;
        public CreateAreaComPositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateAreaComPositionCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int, int>> Handle(CreateAreaComPositionCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing CreateAreaComPositionCommandHandler method 
           var item = _mapper.Map<AreaComPosition>(request);
           _context.AreaComPositions.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  Result<int,int>.Success(item.AreaId,item.ComPositionId);
        }
    }
}
