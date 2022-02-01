// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.Vats.Caching;
using CleanArchitecture.Razor.Application.Features.References.Vats.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Commands.Update
{
    public class UpdateVatCommand : VatDto, IRequest<Result>, IMapFrom<Vat>
    {
        public string CacheKey => VatCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => VatCacheTokenSource.ResetCacheToken;
    }

    public class UpdateVatCommandHandler : IRequestHandler<UpdateVatCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateVatCommandHandler> _localizer;
        public UpdateVatCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateVatCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateVatCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UpdateVatCommandHandler method 
            var item = await _context.Vats.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item != null)
            {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Result.Success();
        }
    }
}
