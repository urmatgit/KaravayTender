using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.Caching;
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.Commands.Delete
{
    public class DeleteQualityDocCommand: IRequest<Result>
    {
      public int Id {  get; set; }
       public string CacheKey => QualityDocCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => QualityDocCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedQualityDocsCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
       public string CacheKey => QualityDocCacheKey.GetAllCacheKey;

       public CancellationTokenSource ResetCacheToken => QualityDocCacheTokenSource.ResetCacheToken;
    }

    public class DeleteQualityDocCommandHandler : 
                 IRequestHandler<DeleteQualityDocCommand, Result>,
                 IRequestHandler<DeleteCheckedQualityDocsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteQualityDocCommandHandler> _localizer;
        public DeleteQualityDocCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteQualityDocCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteQualityDocCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteQualityDocCommandHandler method 
           var item = await _context.QualityDocs.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.QualityDocs.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedQualityDocsCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing DeleteCheckedQualityDocsCommandHandler method 
           var items = await _context.QualityDocs.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.QualityDocs.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
