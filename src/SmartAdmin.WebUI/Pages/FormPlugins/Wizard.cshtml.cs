// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.FormPlugins
{
    public class WizardModel : PageModel
    {
        private readonly ILogger<WizardModel> _logger;

        public WizardModel(ILogger<WizardModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
