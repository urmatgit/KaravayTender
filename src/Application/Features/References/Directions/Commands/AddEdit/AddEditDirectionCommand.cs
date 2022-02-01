// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Directions.Caching;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Directions.Commands.AddEdit
{
    public class AddEditDirectionCommand : DirectionDto, IRequest<Result<int>>, IMapFrom<Direction>
    {
        public string CacheKey => DirectionCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => DirectionCacheTokenSource.ResetCacheToken;
    }

    public class AddEditDirectionCommandHandler : IRequestHandler<AddEditDirectionCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditDirectionCommandHandler> _localizer;
        public AddEditDirectionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditDirectionCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditDirectionCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing AddEditDirectionCommandHandler method 
            if (request.Id > 0)
            {
                var item = await _context.Directions.FindAsync(new object[] { request.Id }, cancellationToken);
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<Direction>(request);
                _context.Directions.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }

        }
    }
}
