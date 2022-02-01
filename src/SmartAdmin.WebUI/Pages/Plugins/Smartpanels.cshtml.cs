// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Plugins
{
    public class SmartpanelsModel : PageModel
    {
        private readonly ILogger<SmartpanelsModel> _logger;

        public SmartpanelsModel(ILogger<SmartpanelsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
