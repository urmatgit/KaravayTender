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

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.AddEdit
{
    public class AddEditCategoryCommand: CategoryDto,IRequest<Result<int>>, IMapFrom<Category>
    {
        public string CacheKey => CategoryCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => CategoryCacheTokenSource.ResetCacheToken;
    }

    public class AddEditCategoryCommandHandler : IRequestHandler<AddEditCategoryCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditCategoryCommandHandler> _localizer;
        public AddEditCategoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditCategoryCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditCategoryCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditCategoryCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<Category>(request);
                _context.Categories.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }
}
