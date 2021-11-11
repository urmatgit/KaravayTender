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

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Commands.Create
{
    public class CreateUnitOfCommand : UnitOfDto, IRequest<Result<int>>, IMapFrom<UnitOf>
    {
        public string CacheKey => UnitOfCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => UnitOfCacheTokenSource.ResetCacheToken;
    }

    public class CreateUnitOfCommandHandler : IRequestHandler<CreateUnitOfCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateUnitOfCommand> _localizer;
        public CreateUnitOfCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateUnitOfCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateUnitOfCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing CreateUnitOfCommandHandler method 
            var item = _mapper.Map<UnitOf>(request);
            _context.UnitOfs.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
    }
}
