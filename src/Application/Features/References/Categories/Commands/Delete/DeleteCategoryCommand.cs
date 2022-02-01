// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Categories.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.Delete
{
    public class DeleteCategoryCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string CacheKey => CategoryCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => CategoryCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedCategoriesCommand : IRequest<Result>
    {
        public int[] Id { get; set; }
        public string CacheKey => CategoryCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => CategoryCacheTokenSource.ResetCacheToken;
    }

    public class DeleteCategoryCommandHandler :
                 IRequestHandler<DeleteCategoryCommand, Result>,
                 IRequestHandler<DeleteCheckedCategoriesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteCategoryCommandHandler> _localizer;
        public DeleteCategoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteCategoryCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DeleteCategoryCommandHandler method 
            var item = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.Categories.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedCategoriesCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DeleteCheckedCategoriesCommandHandler method 
            var items = await _context.Categories.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.Categories.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
