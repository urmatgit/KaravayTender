using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.Caching;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.Update
{
    public class UpdateContragentCategoryCommand: ContragentCategoryDto,IRequest<Result>, IMapFrom<ContragentCategory>
    {
         public string CacheKey => ContragentCategoryCacheKey.GetAllCacheKey;

         public CancellationTokenSource ResetCacheToken => ContragentCategoryCacheTokenSource.ResetCacheToken;
    }

    public class UpdateContragentCategoryCommandHandler : IRequestHandler<UpdateContragentCategoryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateContragentCategoryCommandHandler> _localizer;
        public UpdateContragentCategoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateContragentCategoryCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateContragentCategoryCommand request, CancellationToken cancellationToken)
        {
           //TODO:Implementing UpdateContragentCategoryCommandHandler method 
           var item =await _context.ContragentCategories.FindAsync( new object[] { request.ContragentId,request.CategoryId }, cancellationToken);
           if (item != null)
           {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
           }
           return Result.Success();
        }
    }
}
