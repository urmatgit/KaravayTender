// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.StatusLogs.DTOs;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StatusLogs.Queries.GetAll
{
    public class GetAllStatusLogsQuery : IRequest<IEnumerable<StatusLogDto>>
    {

    }

    public class GetAllStatusLogsQueryHandler :
         IRequestHandler<GetAllStatusLogsQuery, IEnumerable<StatusLogDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllStatusLogsQueryHandler> _localizer;

        public GetAllStatusLogsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllStatusLogsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public Task<IEnumerable<StatusLogDto>> Handle(GetAllStatusLogsQuery request, CancellationToken cancellationToken)
        {
            //TODO:Implementing GetAllStatusLogsQueryHandler method 
            throw new NotImplementedException();
        }
    }
}

