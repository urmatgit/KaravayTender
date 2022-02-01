// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.Caching;
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Commands.Update
{
    public class UpdateUnitOfCommand : UnitOfDto, IRequest<Result>, IMapFrom<UnitOf>
    {
        public string CacheKey => UnitOfCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => UnitOfCacheTokenSource.ResetCacheToken;
    }

    public class UpdateUnitOfCommandHandler : IRequestHandler<UpdateUnitOfCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateUnitOfCommandHandler> _localizer;
        public UpdateUnitOfCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateUnitOfCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateUnitOfCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UpdateUnitOfCommandHandler method 
            var item = await _context.UnitOfs.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item != null)
            {
                item = _mapper.Map(request, item);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Result.Success();
        }
    }
}
