using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Caching;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.Update
{
    public class UpdateNomenclatureCommand: NomenclatureDto,IRequest<Result>, IMapFrom<Nomenclature>
    {
         public string CacheKey => NomenclatureCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => NomenclatureCacheTokenSource.ResetCacheToken;
    }

    public class UpdateNomenclatureCommandHandler : IRequestHandler<UpdateNomenclatureCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateNomenclatureCommandHandler> _localizer;
        public UpdateNomenclatureCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateNomenclatureCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateNomenclatureCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateNomenclatureCommandHandler method 
           var item =await _context.Nomenclatures.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
