using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.Caching;
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Commands.Delete
{
    public class DeleteUnitOfCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => UnitOfCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => UnitOfCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedUnitOfsCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => UnitOfCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => UnitOfCacheTokenSource.ResetCacheToken;
    }

    public class DeleteUnitOfCommandHandler : 
                 IRequestHandler<DeleteUnitOfCommand, Result>,
                 IRequestHandler<DeleteCheckedUnitOfsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteUnitOfCommandHandler> _localizer;
        public DeleteUnitOfCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteUnitOfCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteUnitOfCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteUnitOfCommandHandler method 
           var item = await _context.UnitOfs.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.UnitOfs.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedUnitOfsCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedUnitOfsCommandHandler method 
           var items = await _context.UnitOfs.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.UnitOfs.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
