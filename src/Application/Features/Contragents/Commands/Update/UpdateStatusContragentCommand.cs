// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.Update
{
    public class UpdateStatusContragentCommand :   IRequest<Result>
    {
        public int Id { get; set; }
        public ContragentStatus Status { get; set; }
    }
    public class UpdateStatusContragentCommandHandler : IRequestHandler<UpdateStatusContragentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateStatusContragentCommandHandler> _localizer;
        public UpdateStatusContragentCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateStatusContragentCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(UpdateStatusContragentCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing UpdateContragentCommandHandler method 
            var item = await _context.Contragents.FindAsync(new object[] { request.Id }, cancellationToken);
            if (item != null)
            {
                item.Status = request.Status;
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Result.Success();
        }
    }
}
