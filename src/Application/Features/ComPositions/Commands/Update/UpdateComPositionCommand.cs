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

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Commands.Update
{
    public class UpdateComPositionCommand: ComPositionDto,IRequest<Result>, IMapFrom<ComPosition>
    {
         public string CacheKey => ComPositionCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => ComPositionCacheTokenSource.ResetCacheToken;
    }

    public class UpdateComPositionCommandHandler : IRequestHandler<UpdateComPositionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateComPositionCommandHandler> _localizer;
        public UpdateComPositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateComPositionCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateComPositionCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateComPositionCommandHandler method 
           var item =await _context.ComPositions.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
