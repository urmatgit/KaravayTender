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
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Queries.GetAll
{
   public class GetContragentIdByUserIdQuery : IRequest<Result <int>>
    {
       
    }
    public class GetContragentIdByUserIdQueryHandler : IRequestHandler<GetContragentIdByUserIdQuery, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetContragentIdByUserIdQueryHandler(
            IApplicationDbContext context,
             ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<Result<int>> Handle(GetContragentIdByUserIdQuery request, CancellationToken cancellationToken)
        {
            int ContragentId = 0;
            var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (currentUser is null)
            {
                throw new Exception("Пользователь не найден!");
            }
            if (currentUser.ContragentId.HasValue)
            {
                ContragentId = currentUser.ContragentId.Value;

            }
            else
            {
                var contragent = await _context.Contragents.FirstOrDefaultAsync(c => c.ApplicationUserId == currentUser.Id, cancellationToken);
                if (contragent is null)
                    throw new Exception("Контрагент не найден!");
                ContragentId = contragent.Id;
            }
            return Result<int>.Success(ContragentId);
        }
    }
}
