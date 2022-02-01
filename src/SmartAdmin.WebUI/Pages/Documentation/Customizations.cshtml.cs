// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Documentation
{
    public class CustomizationsModel : PageModel
    {
        private readonly ILogger<CustomizationsModel> _logger;

        public CustomizationsModel(ILogger<CustomizationsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
