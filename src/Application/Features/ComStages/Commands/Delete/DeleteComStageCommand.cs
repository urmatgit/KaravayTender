using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComStages.Caching;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Commands.Delete
{
    public class DeleteComStageCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => ComStageCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => ComStageCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedComStagesCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => ComStageCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => ComStageCacheTokenSource.ResetCacheToken;
    }

    public class DeleteComStageCommandHandler : 
                 IRequestHandler<DeleteComStageCommand, Result>,
                 IRequestHandler<DeleteCheckedComStagesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteComStageCommandHandler> _localizer;
        public DeleteComStageCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteComStageCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteComStageCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteComStageCommandHandler method 
           var item = await _context.ComStages.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.ComStages.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedComStagesCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedComStagesCommandHandler method 
           var items = await _context.ComStages.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.ComStages.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
