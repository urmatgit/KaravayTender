using System.Linq;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Directions.Commands.Delete
{
    public class DeleteDirectionCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => DirectionCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => DirectionCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedDirectionsCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => DirectionCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => DirectionCacheTokenSource.ResetCacheToken;
    }

    public class DeleteDirectionCommandHandler : 
                 IRequestHandler<DeleteDirectionCommand, Result>,
                 IRequestHandler<DeleteCheckedDirectionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteDirectionCommandHandler> _localizer;
        public DeleteDirectionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteDirectionCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteDirectionCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteDirectionCommandHandler method 
           var item = await _context.Directions.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.Directions.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedDirectionsCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedDirectionsCommandHandler method 
           var items = await _context.Directions.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.Directions.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
