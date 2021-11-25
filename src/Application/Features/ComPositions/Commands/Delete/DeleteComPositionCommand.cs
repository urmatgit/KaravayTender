using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComPositions.Caching;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Commands.Delete
{
    public class DeleteComPositionCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => ComPositionCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => ComPositionCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedComPositionsCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => ComPositionCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => ComPositionCacheTokenSource.ResetCacheToken;
    }

    public class DeleteComPositionCommandHandler : 
                 IRequestHandler<DeleteComPositionCommand, Result>,
                 IRequestHandler<DeleteCheckedComPositionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteComPositionCommandHandler> _localizer;
        public DeleteComPositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteComPositionCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteComPositionCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteComPositionCommandHandler method 
           var item = await _context.ComPositions.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.ComPositions.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedComPositionsCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedComPositionsCommandHandler method 
           var items = await _context.ComPositions.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.ComPositions.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
