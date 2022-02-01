// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.Caching;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.Create
{
    public class CreateContragentCategoryCommand : ContragentCategoryDto, IRequest<Result<int, int>>, IMapFrom<ContragentCategory>
    {
        public string CacheKey => ContragentCategoryCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ContragentCategoryCacheTokenSource.ResetCacheToken;
    }

    public class CreateContragentCategoryCommandHandler : IRequestHandler<CreateContragentCategoryCommand, Result<int, int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateContragentCategoryCommand> _localizer;
        public CreateContragentCategoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateContragentCategoryCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int, int>> Handle(CreateContragentCategoryCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing CreateContragentCategoryCommandHandler method 
            var item = _mapper.Map<ContragentCategory>(request);
            _context.ContragentCategories.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int, int>.Success(item.ContragentId, item.CategoryId);
        }
    }
}
