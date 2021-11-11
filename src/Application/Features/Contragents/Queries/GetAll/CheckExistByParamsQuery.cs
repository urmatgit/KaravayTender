// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Queries.GetAll
{
    public class CheckExistByParamsQuery : IRequest<Result<ContragentDto>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string INN { get; set; }
    }
    public class CheckExistByParamsQueryHandler : IRequestHandler<CheckExistByParamsQuery, Result<ContragentDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CheckExistByParamsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<ContragentDto>> Handle(CheckExistByParamsQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Contragents
                        .Where(c => c.INN == request.INN || c.Name == request.Name || c.Email == request.Email)
                        .ProjectTo<ContragentDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);

            return Result<ContragentDto>.Success(data);
        }
    }
}
