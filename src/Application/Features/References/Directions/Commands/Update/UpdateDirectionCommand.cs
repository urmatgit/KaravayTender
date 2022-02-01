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

namespace CleanArchitecture.Razor.Application.Features.Directions.Commands.Update
{
    public class UpdateDirectionCommand : DirectionDto, IRequest<Result>, IMapFrom<Direction>
    {
        public string CacheKey => DirectionCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => DirectionCacheTokenSource.ResetCacheToken;
    }

    public class UpdateDirectionCommandHandler : IRequestHandler<UpdateDirectionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateDirectionCommandHandler> _localizer;
        public UpdateDirectionCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateDirectionCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateDirectionCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UpdateDirectionCommandHandler method 
            var item = await _context.Directions.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item != null)
            {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Result.Success();
        }
    }
}
