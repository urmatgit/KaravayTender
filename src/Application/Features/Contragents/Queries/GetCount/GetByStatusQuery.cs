// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Queries.GetCount
{
    public class GetByStatusQuery : IRequest<Result<int>>
    {
        public ContragentStatus Status { get; set; }
    }
    public class GetByStatusQueryHandler : IRequestHandler<GetByStatusQuery, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        public GetByStatusQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<int>> Handle(GetByStatusQuery request, CancellationToken cancellationToken)
        {
            var count = await _context.Contragents
                       .Where(c => c.Status == request.Status)
                       .CountAsync(cancellationToken);
            return Result<int>.Success(count);
        }
    }
}
