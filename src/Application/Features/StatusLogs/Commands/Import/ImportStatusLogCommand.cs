// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.StatusLogs.DTOs;
using CleanArchitecture.Razor.Domain.Entities.Karavay;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StatusLogs.Commands.Import
{
    public class ImportStatusLogCommand : StatusLogDto, IRequest<Result>, IMapFrom<StatusLog>
    {

    }

    public class ImportStatusLogCommandHandler : IRequestHandler<ImportStatusLogCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImportStatusLogCommandHandler> _localizer;
        public ImportStatusLogCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<ImportStatusLogCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(ImportStatusLogCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing ImportStatusLogCommandHandler method 
            throw new System.NotImplementedException();
        }
    }
}
