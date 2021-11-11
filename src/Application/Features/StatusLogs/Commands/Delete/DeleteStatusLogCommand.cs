// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Features.StatusLogs.Commands.Delete
{
    public class DeleteStatusLogCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class DeleteCheckedStatusLogsCommand : IRequest<Result>
    {
        public int[] Id { get; set; }
    }

    public class DeleteStatusLogCommandHandler :
                 IRequestHandler<DeleteStatusLogCommand, Result>,
                 IRequestHandler<DeleteCheckedStatusLogsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteStatusLogCommandHandler> _localizer;
        public DeleteStatusLogCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteStatusLogCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteStatusLogCommand request, CancellationToken cancellationToken)
        {

            var item = await _context.StatusLogs.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.StatusLogs.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedStatusLogsCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.StatusLogs.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.StatusLogs.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
    }
}
