// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.Delete
{
    public class DeleteContragentCategoryCommand : IRequest<Result>
    {
        public int ContragentId { get; set; }
        public int CategoryId { get; set; }

        public string CacheKey => ContragentCategoryCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ContragentCategoryCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedContragentCategoriesCommand : IRequest<Result>
    {
        public int[] ContragentId { get; set; }
        public int[] CategoryId { get; set; }
        public string CacheKey => ContragentCategoryCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ContragentCategoryCacheTokenSource.ResetCacheToken;
    }

    public class DeleteContragentCategoryCommandHandler :
                 IRequestHandler<DeleteContragentCategoryCommand, Result>,
                 IRequestHandler<DeleteCheckedContragentCategoriesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteContragentCategoryCommandHandler> _localizer;
        public DeleteContragentCategoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteContragentCategoryCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteContragentCategoryCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DeleteContragentCategoryCommandHandler method 
            var item = await _context.ContragentCategories.FindAsync(new object[] { request.ContragentId, request.CategoryId }, cancellationToken);
            _context.ContragentCategories.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedContragentCategoriesCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DeleteCheckedContragentCategoriesCommandHandler method 
            var items = await _context.ContragentCategories.Where(x => request.ContragentId.Contains(x.ContragentId) && request.CategoryId.Contains(x.CategoryId)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.ContragentCategories.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
