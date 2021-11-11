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

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.AddEdit
{
    public class AddEditNomenclatureCommand: NomenclatureDto,IRequest<Result<int>>, IMapFrom<Nomenclature>
    {
        public string CacheKey => NomenclatureCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => NomenclatureCacheTokenSource.ResetCacheToken;
    }

    public class AddEditNomenclatureCommandHandler : IRequestHandler<AddEditNomenclatureCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditNomenclatureCommandHandler> _localizer;
        public AddEditNomenclatureCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditNomenclatureCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditNomenclatureCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditNomenclatureCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.Nomenclatures.FindAsync(new object[] { request.Id }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<Nomenclature>(request);
                _context.Nomenclatures.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }
}
