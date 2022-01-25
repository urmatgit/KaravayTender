// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Queries.GetAll
{
    public class GetParticipantRequestCountCommand : IRequest<int?>
    {

    }
    public class GetParticipantRequestCountCommandHandler : IRequestHandler<GetParticipantRequestCountCommand, int?>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GetParticipantRequestCountCommandHandler> _logger;
        public GetParticipantRequestCountCommandHandler(
             IApplicationDbContext context,
              ICurrentUserService currentUserService,
         UserManager<ApplicationUser> userManager,
         ILogger<GetParticipantRequestCountCommandHandler> logger
            )
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _logger = logger;

        }
        public async Task<int?> Handle(GetParticipantRequestCountCommand request, CancellationToken cancellationToken)
        {
            var ContragentId = await GetContragentId(cancellationToken); 
            try
            {
                var count = await _context.GetComOfferWithLastStage(contragentId: ContragentId).CountAsync(cancellationToken);
                    
                return count ;
            }catch(Exception er)
            {
                _logger.LogError("GetComOfferWithLastStage", er);
                return null;
            }
        }
        public async Task<int> GetContragentId(CancellationToken cancellationToken)
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
                {
                    _logger.LogError("Контрагент не найден!");
                    throw new Exception("Контрагент не найден!");
                }
                ContragentId = contragent.Id;
            }
            return ContragentId;
        }
    }

}
