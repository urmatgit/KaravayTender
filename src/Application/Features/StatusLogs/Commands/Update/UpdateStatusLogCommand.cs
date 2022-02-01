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

namespace CleanArchitecture.Razor.Application.Features.StatusLogs.Commands.Update
{
    public class UpdateStatusLogCommand : StatusLogDto, IRequest<Result>, IMapFrom<StatusLog>
    {

    }

    public class UpdateStatusLogCommandHandler : IRequestHandler<UpdateStatusLogCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateStatusLogCommandHandler> _localizer;
        public UpdateStatusLogCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateStatusLogCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateStatusLogCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UpdateStatusLogCommandHandler method 
            throw new System.NotImplementedException();
        }
    }
}
