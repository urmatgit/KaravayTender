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
using CleanArchitecture.Razor.Application.Features.ComStages.Commands.Create;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Update
{
    public class CreateRequestWinnerCommand : IRequest<Result>
    {
        public int ContragentId  { get; set; }
        public int ComOfferId { get; set; }
        public DateTime Deadline { get; set; }
    }
    public class CreateRequestWinnerCommandHandler : IRequestHandler<CreateRequestWinnerCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateStageCompositionPricesCommandHandler> _localizer;
        
        private readonly IMediator _mediator;
        public CreateRequestWinnerCommandHandler(
            IApplicationDbContext context,
            
            IMediator mediator,
            IStringLocalizer<UpdateStageCompositionPricesCommandHandler> localizer,
             IMapper mapper)
        {
            
            _context = context;
            
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<Result> Handle(CreateRequestWinnerCommand request, CancellationToken cancellationToken)
        {
            var next = await _mediator.Send(new NextComStageWinnerCommand() { ComOfferId = request.ComOfferId, DeadlineDate = request.Deadline,ContragentId=request.ContragentId }, cancellationToken);
            if (next is null)
                return Result.Failure(next.Errors);
            return Result.Success();
        }
    }
}
