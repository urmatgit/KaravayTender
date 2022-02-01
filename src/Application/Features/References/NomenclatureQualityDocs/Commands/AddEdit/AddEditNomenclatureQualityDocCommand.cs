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

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Commands.AddEdit
{
    public class AddEditNomenclatureQualityDocCommand: NomenclatureQualityDocDto,IRequest<Result<int,int>>, IMapFrom<NomenclatureQualityDoc>
    {
        public string CacheKey => NomenclatureQualityDocCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => NomenclatureQualityDocCacheTokenSource.ResetCacheToken;
    }

    public class AddEditNomenclatureQualityDocCommandHandler : IRequestHandler<AddEditNomenclatureQualityDocCommand, Result<int,int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditNomenclatureQualityDocCommandHandler> _localizer;
        public AddEditNomenclatureQualityDocCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditNomenclatureQualityDocCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int,int>> Handle(AddEditNomenclatureQualityDocCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditNomenclatureQualityDocCommandHandler method 
            if (request.NomenclatureId > 0 && request.QualityDocId > 0)
            {
                var item = await _context.NomenclatureQualityDocs.FindAsync(new object[] { request.NomenclatureId, request.QualityDocId }, cancellationToken);
                if (item != null)
                {
                    item = _mapper.Map<NomenclatureQualityDoc>(request);
                    _context.NomenclatureQualityDocs.Add(item);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                return Result<int, int>.Success(item.NomenclatureId, item.QualityDocId);
            }
            //else
            //{
            //    var item = _mapper.Map<ContragentCategory>(request);
            //    _context.ContragentCategories.Add(item);
            //    await _context.SaveChangesAsync(cancellationToken);
            return Result<int, int>.Success(request.NomenclatureId, request.QualityDocId);

        }
    }
}
