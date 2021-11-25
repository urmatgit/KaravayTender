using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Caching;
using CleanArchitecture.Razor.Application.Features.StageCompositions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Update
{
    public class UpdateStageCompositionCommand: StageCompositionDto,IRequest<Result>, IMapFrom<StageComposition>
    {
         public string CacheKey => StageCompositionCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => StageCompositionCacheTokenSource.ResetCacheToken;
    }

    public class UpdateStageCompositionCommandHandler : IRequestHandler<UpdateStageCompositionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateStageCompositionCommandHandler> _localizer;
        public UpdateStageCompositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateStageCompositionCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateStageCompositionCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateStageCompositionCommandHandler method 
           var item =await _context.StageCompositions.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
