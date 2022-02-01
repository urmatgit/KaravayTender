// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Plugins
{
    public class NavigationModel : PageModel
    {
        private readonly ILogger<NavigationModel> _logger;

        public NavigationModel(ILogger<NavigationModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
