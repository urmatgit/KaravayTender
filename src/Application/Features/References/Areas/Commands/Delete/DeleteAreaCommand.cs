using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Caching;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.Areas.Caching;
using CleanArchitecture.Razor.Application.Features.References.Areas.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Commands.Delete
{
    public class DeleteAreaCommand: IRequest<Result>//, ICacheInvalidator
    {
      public int Id {  get; set; }
       public string CacheKey => AreaCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => AreaCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedAreasCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => AreaCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => AreaCacheTokenSource.ResetCacheToken;
    }

    public class DeleteAreaCommandHandler : 
                 IRequestHandler<DeleteAreaCommand, Result>,
                 IRequestHandler<DeleteCheckedAreasCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteAreaCommandHandler> _localizer;
        public DeleteAreaCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteAreaCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteAreaCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteAreaCommandHandler method 
           var item = await _context.Areas.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.Areas.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedAreasCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedAreasCommandHandler method 
           var items = await _context.Areas.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.Areas.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
