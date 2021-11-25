using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ComStages.Caching;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Commands.AddEdit
{
    public class AddEditComStageCommand: ComStageDto,IRequest<Result<int>>, IMapFrom<ComStage>
    {
        public string CacheKey => ComStageCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ComStageCacheTokenSource.ResetCacheToken;
    }

    public class AddEditComStageCommandHandler : IRequestHandler<AddEditComStageCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditComStageCommandHandler> _localizer;
        public AddEditComStageCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditComStageCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditComStageCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditComStageCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.ComStages.FindAsync(new object[] { request.Id }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<ComStage>(request);
                _context.ComStages.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }
}
