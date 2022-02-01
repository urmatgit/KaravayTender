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

namespace CleanArchitecture.Razor.Application.Features.AreaComPositions.Commands.AddEdit
{
    public class AddEditAreaComPositionCommand: AreaComPositionDto,IRequest<Result<int, int>>, IMapFrom<AreaComPosition>
    {
        public string CacheKey => AreaComPositionCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => AreaComPositionCacheTokenSource.ResetCacheToken;
    }

    public class AddEditAreaComPositionCommandHandler : IRequestHandler<AddEditAreaComPositionCommand, Result<int, int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditAreaComPositionCommandHandler> _localizer;
        public AddEditAreaComPositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditAreaComPositionCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int, int>> Handle(AddEditAreaComPositionCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditAreaComPositionCommandHandler method 
            if (request.AreaId > 0 && request.ComPositionId>0)
            {
                var item = await _context.AreaComPositions.FindAsync(new object[] { request.AreaId,request.ComPositionId }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int, int>.Success(item.AreaId,item.ComPositionId);
            }
            else
            {
                var item = _mapper.Map<AreaComPosition>(request);
                _context.AreaComPositions.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int, int>.Success(item.AreaId,item.ComPositionId);
            }
           
        }
    }
}
