// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Settings
{
    public class HowItWorksModel : PageModel
    {
        private readonly ILogger<HowItWorksModel> _logger;

        public HowItWorksModel(ILogger<HowItWorksModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
