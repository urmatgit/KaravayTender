using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Caching;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.Delete
{
    public class DeleteNomenclatureCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => NomenclatureCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => NomenclatureCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedNomenclaturesCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => NomenclatureCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => NomenclatureCacheTokenSource.ResetCacheToken;
    }

    public class DeleteNomenclatureCommandHandler : 
                 IRequestHandler<DeleteNomenclatureCommand, Result>,
                 IRequestHandler<DeleteCheckedNomenclaturesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteNomenclatureCommandHandler> _localizer;
        public DeleteNomenclatureCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteNomenclatureCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteNomenclatureCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteNomenclatureCommandHandler method 
           var item = await _context.Nomenclatures.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.Nomenclatures.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedNomenclaturesCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedNomenclaturesCommandHandler method 
           var items = await _context.Nomenclatures.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.Nomenclatures.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
