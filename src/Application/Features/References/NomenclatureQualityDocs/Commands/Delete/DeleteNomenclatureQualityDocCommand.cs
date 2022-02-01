using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Caching;
using CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Commands.Delete
{
    public class DeleteNomenclatureQualityDocCommand: IRequest<Result>
    {
      public int NomenclatureId {  get; set; }
        public int QualityDocId { get; set; }
        public string CacheKey => NomenclatureQualityDocCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => NomenclatureQualityDocCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedNomenclatureQualityDocsCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => NomenclatureQualityDocCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => NomenclatureQualityDocCacheTokenSource.ResetCacheToken;
    }

    public class DeleteNomenclatureQualityDocCommandHandler : 
                 IRequestHandler<DeleteNomenclatureQualityDocCommand, Result>,
                 IRequestHandler<DeleteCheckedNomenclatureQualityDocsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteNomenclatureQualityDocCommandHandler> _localizer;
        public DeleteNomenclatureQualityDocCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteNomenclatureQualityDocCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteNomenclatureQualityDocCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteNomenclatureQualityDocCommandHandler method 
           var item = await _context.NomenclatureQualityDocs.FindAsync(new object[] { request.NomenclatureId,request.QualityDocId }, cancellationToken);
            _context.NomenclatureQualityDocs.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedNomenclatureQualityDocsCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedNomenclatureQualityDocsCommandHandler method 
           var items = await _context.NomenclatureQualityDocs.Where(x => request.Id.Contains(x.NomenclatureId) && request.Id.Contains(x.QualityDocId)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.NomenclatureQualityDocs.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
