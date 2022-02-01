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

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Commands.Create
{
    public class CreateNomenclatureQualityDocCommand: NomenclatureQualityDocDto,IRequest<Result<int,int>>, IMapFrom<NomenclatureQualityDoc>
    {
        public string CacheKey => NomenclatureQualityDocCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => NomenclatureQualityDocCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateNomenclatureQualityDocCommandHandler : IRequestHandler<CreateNomenclatureQualityDocCommand, Result<int,int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateNomenclatureQualityDocCommand> _localizer;
        public CreateNomenclatureQualityDocCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateNomenclatureQualityDocCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int,int>> Handle(CreateNomenclatureQualityDocCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing CreateNomenclatureQualityDocCommandHandler method 
           var item = _mapper.Map<NomenclatureQualityDoc>(request);
           _context.NomenclatureQualityDocs.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  Result<int,int>.Success(item.NomenclatureId,item.QualityDocId);
        }
    }
}
