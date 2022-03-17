using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Features.ComOffers.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.ComPositions.Commands.AddEdit;
using CleanArchitecture.Razor.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace SmartAdmin.WebUI.Pages.ComParticipantsEdit
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public AddEditComOfferCommand Input { get; set; }
        [BindProperty]
        public AddEditComPositionCommand InputPos { get; set; }

        private readonly IIdentityService _identityService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _mediator;
        private readonly IStringLocalizer<SmartAdmin.WebUI.Pages.ComOffers.IndexModel> _localizer;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
           IIdentityService identityService,
            IAuthorizationService authorizationService,
            ICurrentUserService currentUserService,
            ISender mediator,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<SmartAdmin.WebUI.Pages.ComOffers.IndexModel> localizer
            )
        {
            _userManager = userManager;
            _identityService = identityService;
            _authorizationService = authorizationService;
            _currentUserService = currentUserService;
            _mediator = mediator;
            _localizer = localizer;
            var email = _currentUserService.UserId;
        }
    }
}
