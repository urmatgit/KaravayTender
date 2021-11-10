using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Categories.Caching;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.Update
{
    public class UpdateCategoryCommand: CategoryDto,IRequest<Result>, IMapFrom<Category>
    {
         public string CacheKey => CategoryCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => CategoryCacheTokenSource.ResetCacheToken;
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateCategoryCommandHandler> _localizer;
        public UpdateCategoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateCategoryCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateCategoryCommandHandler method 
           var item =await _context.Categories.FindAsync( new object[] { request.Id }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
