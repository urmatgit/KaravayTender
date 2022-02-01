using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.Areas.Caching;
using CleanArchitecture.Razor.Application.Features.References.Areas.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using CleanArchitecture.Razor.Domain.Events;

using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Commands.Update
{
    public class UpdateAreaCommand: AreaDto,IRequest<Result>, IMapFrom<Area>
    {
         public string CacheKey => AreaCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => AreaCacheTokenSource.ResetCacheToken;
    }

    public class UpdateAreaCommandHandler : IRequestHandler<UpdateAreaCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateAreaCommandHandler> _localizer;
        public UpdateAreaCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateAreaCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateAreaCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateAreaCommandHandler method 
           var item =await _context.Areas.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
