using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.Areas.Caching;
using CleanArchitecture.Razor.Application.Features.References.Areas.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Commands.AddEdit
{
    public class AddEditAreaCommand: AreaDto,IRequest<Result<int>>, IMapFrom<Area>
    {
        public string CacheKey => AreaCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => AreaCacheTokenSource.ResetCacheToken;
    }

    public class AddEditAreaCommandHandler : IRequestHandler<AddEditAreaCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditAreaCommandHandler> _localizer;
        public AddEditAreaCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditAreaCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditAreaCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditAreaCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.Areas.FindAsync(new object[] { request.Id }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<Area>(request);
                _context.Areas.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }
}
