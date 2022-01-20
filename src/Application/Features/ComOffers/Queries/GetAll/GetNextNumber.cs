// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Queries.GetAll
{
    public class GetNextNumberCommand : IRequest<int>
    {

    }
    public class GetNextNumberHandler : IRequestHandler<GetNextNumberCommand, int>
    {
        private readonly IApplicationDbContext _context;
        public GetNextNumberHandler(
             IApplicationDbContext context
            )
        {
            _context = context;
        }
        public async Task<int> Handle(GetNextNumberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maxNumber = await _context.ComOffers.MaxAsync(x => x.Number, cancellationToken);
                return maxNumber + 1;
            }catch(Exception er)
            {
                return 1;
            }
        }
    }

}
