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
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Commands.Update
{
    public class UpdateNomenclatureQualityDocCommand: NomenclatureQualityDocDto,IRequest<Result>, IMapFrom<NomenclatureQualityDoc>
    {
         public string CacheKey => NomenclatureQualityDocCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => NomenclatureQualityDocCacheTokenSource.ResetCacheToken;
    }

    public class UpdateNomenclatureQualityDocCommandHandler : IRequestHandler<UpdateNomenclatureQualityDocCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateNomenclatureQualityDocCommandHandler> _localizer;
        public UpdateNomenclatureQualityDocCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateNomenclatureQualityDocCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateNomenclatureQualityDocCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateNomenclatureQualityDocCommandHandler method 
           var item =await _context.NomenclatureQualityDocs.FindAsync( new object[] { request.NomenclatureId,request.QualityDocId }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
