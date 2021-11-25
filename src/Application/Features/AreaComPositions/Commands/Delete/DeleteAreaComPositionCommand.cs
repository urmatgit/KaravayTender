using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.AreaComPositions.Caching;
using CleanArchitecture.Razor.Application.Features.AreaComPositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.AreaComPositions.Commands.Delete
{
    public class DeleteAreaComPositionCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => AreaComPositionCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => AreaComPositionCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedAreaComPositionsCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => AreaComPositionCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => AreaComPositionCacheTokenSource.ResetCacheToken;
    }

    public class DeleteAreaComPositionCommandHandler : 
                 IRequestHandler<DeleteAreaComPositionCommand, Result>,
                 IRequestHandler<DeleteCheckedAreaComPositionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteAreaComPositionCommandHandler> _localizer;
        public DeleteAreaComPositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteAreaComPositionCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteAreaComPositionCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteAreaComPositionCommandHandler method 
           var item = await _context.AreaComPositions.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.AreaComPositions.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedAreaComPositionsCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedAreaComPositionsCommandHandler method 
           //var items = await _context.AreaComPositions.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
           // foreach (var item in items)
           // {
           //     _context.AreaComPositions.Remove(item);
           // }
           // await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
