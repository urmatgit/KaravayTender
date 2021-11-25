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

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.AddEdit
{
    public class AddEditStageCompositionCommand: StageCompositionDto,IRequest<Result<StageCompositionDto>>, IMapFrom<StageComposition>
    {
        public string CacheKey => StageCompositionCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => StageCompositionCacheTokenSource.ResetCacheToken;
    }

    public class AddEditStageCompositionCommandHandler : IRequestHandler<AddEditStageCompositionCommand, Result<StageCompositionDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditStageCompositionCommandHandler> _localizer;
        public AddEditStageCompositionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditStageCompositionCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<StageCompositionDto>> Handle(AddEditStageCompositionCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditStageCompositionCommandHandler method 
            if (request.ComStageId>0 && request.ContragentId >0 && request.ComPositionId>0 )
            {
                var item = await _context.StageCompositions.FindAsync(new object[] { request.ComStageId,request.ContragentId,request.ComPositionId }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<StageCompositionDto>.Success(_mapper.Map<StageCompositionDto>(item));
            }
            else
            {
                var item = _mapper.Map<StageComposition>(request);
                _context.StageCompositions.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<StageCompositionDto>.Success(_mapper.Map< StageCompositionDto>(item));
            }
           
        }
    }
}
