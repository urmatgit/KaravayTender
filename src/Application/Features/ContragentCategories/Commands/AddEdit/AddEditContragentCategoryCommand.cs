using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.AddEdit
{
    public class AddEditContragentCategoryCommand: ContragentCategoryDto,IRequest<Result<int>>, IMapFrom<ContragentCategory>
    {
        //public string CacheKey => ContragentCategoryCacheKey.GetAllCacheKey;

        //public CancellationTokenSource ResetCacheToken => ContragentCategoryCacheTokenSource.ResetCacheToken;
    }

    public class AddEditContragentCategoryCommandHandler : IRequestHandler<AddEditContragentCategoryCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditContragentCategoryCommandHandler> _localizer;
        public AddEditContragentCategoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditContragentCategoryCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditContragentCategoryCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditContragentCategoryCommandHandler method 
            if (request > 0)
            {
                var item = await _context.ContragentCategories.FindAsync(new object[] { request.Id }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<ContragentCategory>(request);
                _context.ContragentCategories.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }
}
