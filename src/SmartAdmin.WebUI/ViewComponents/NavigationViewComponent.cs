using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Features.Contragents.Queries.GetCount;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAdmin.WebUI.Models;

namespace SmartAdmin.WebUI.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly string[] _roles;
        private readonly ISender _mediator;
        public NavigationViewComponent(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IIdentityService  identityService,
             ISender mediator
            )
        {
            _mediator = mediator;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _identityService = identityService;
            var userId = _currentUserService.UserId;
            _roles = _userManager.Users.Where(x=>x.Id== userId).Include(x=>x.UserRoles).ThenInclude(x=>x.Role).SelectMany(x=>x.UserRoles).Select(x=>x.Role.Name).ToArray();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var items = NavigationModel.GetNavigation(x=>!x.Roles.Any()  || (x.Roles.Any() && _roles.Any() && x.Roles.Where(x=>_roles.Contains(x)).Any()) );
            var count = await _mediator.Send(new GetByStatusQuery() { Status = ContragentStatus.OnRegistration });
            if (count?.Data > 0)
            {
                items.SpanValues.Add("[ContragentOnRegistrationCount]", count?.Data.ToString());
            }
            return View(items);
        }
    }
}
