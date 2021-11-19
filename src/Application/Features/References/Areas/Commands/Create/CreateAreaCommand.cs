using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.Areas.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using MediatR;
using Microsoft.Extensions.Localization;
using CleanArchitecture.Razor.Application.Features.References.Areas.Caching;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Commands.Create
{
    public class CreateAreaCommand: AreaDto,IRequest<Result<int>>, IMapFrom<Area>
    {
        public string CacheKey => AreaCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => AreaCacheTokenSource.ResetCacheToken;
    }
    
    public class CreateAreaCommandHandler : IRequestHandler<CreateAreaCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateAreaCommand> _localizer;
        public CreateAreaCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateAreaCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateAreaCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing CreateAreaCommandHandler method 
           var item = _mapper.Map<Area>(request);
           _context.Areas.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  Result<int>.Success(item.Id);
        }
    }
}
