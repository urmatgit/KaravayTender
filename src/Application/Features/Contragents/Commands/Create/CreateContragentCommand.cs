// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Contragents.Caching;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.Create
{
    public class CreateContragentCommand : ContragentDto, IRequest<Result<int>>, IMapFrom<Contragent>
    {
        public string CacheKey => ContragentCacheKey.GetAllCacheKey;

        public CancellationTokenSource ResetCacheToken => ContragentCacheTokenSource.ResetCacheToken;
    }

    public class CreateContragentCommandHandler : IRequestHandler<CreateContragentCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateContragentCommand> _localizer;
        public CreateContragentCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateContragentCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateContragentCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing CreateContragentCommandHandler method 
            var item = _mapper.Map<Contragent>(request);
            _context.Contragents.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
    }
}
