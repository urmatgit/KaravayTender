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

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Commands.AddEdit
{
    public class AddEditVatCommand : VatDto, IRequest<Result<int>>, IMapFrom<Vat>
    {
        public string CacheKey => VatCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => VatCacheTokenSource.ResetCacheToken;
    }

    public class AddEditVatCommandHandler : IRequestHandler<AddEditVatCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditVatCommandHandler> _localizer;
        public AddEditVatCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditVatCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditVatCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditVatCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.Vats.FindAsync(new object[] { request.Id }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<Vat>(request);
                _context.Vats.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }

        }
    }
}
