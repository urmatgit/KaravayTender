using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComPositions.Caching;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Commands.AddEdit
{
    public class AddEditComPositionCommand: ComPositionDto,IRequest<Result<int>>, IMapFrom<ComPosition>
    {
        public string CacheKey => ComPositionCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComPositionCacheTokenSource.ResetCacheToken;
    }

    public class AddEditComPositionCommandHandler : IRequestHandler<AddEditComPositionCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditComPositionCommandHandler> _localizer;
        public AddEditComPositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditComPositionCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditComPositionCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditComPositionCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.ComPositions.FindAsync(new object[] { request.Id }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<ComPosition>(request);
                _context.ComPositions.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }
}
