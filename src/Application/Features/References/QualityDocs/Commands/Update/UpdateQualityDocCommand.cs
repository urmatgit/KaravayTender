using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.Caching;
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.Commands.Update
{
    public class UpdateQualityDocCommand: QualityDocDto,IRequest<Result>, IMapFrom<QualityDoc>
    {
         public string CacheKey => QualityDocCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => QualityDocCacheTokenSource.ResetCacheToken;
    }

    public class UpdateQualityDocCommandHandler : IRequestHandler<UpdateQualityDocCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateQualityDocCommandHandler> _localizer;
        public UpdateQualityDocCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateQualityDocCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateQualityDocCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateQualityDocCommandHandler method 
           var item =await _context.QualityDocs.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
