// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.Vats.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Commands.Delete
{
    public class DeleteVatCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string CacheKey => VatCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => VatCacheTokenSource.ResetCacheToken;
    }
    public class DeleteCheckedVatsCommand : IRequest<Result>
    {
        public int[] Id { get; set; }
        public string CacheKey => VatCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => VatCacheTokenSource.ResetCacheToken;
    }

    public class DeleteVatCommandHandler :
                 IRequestHandler<DeleteVatCommand, Result>,
                 IRequestHandler<DeleteCheckedVatsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteVatCommandHandler> _localizer;
        public DeleteVatCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteVatCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteVatCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DeleteVatCommandHandler method 
            var item = await _context.Vats.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.Vats.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedVatsCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DeleteCheckedVatsCommandHandler method 
            var items = await _context.Vats.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.Vats.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
