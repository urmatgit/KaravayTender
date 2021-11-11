// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Docs
{
    public class LicensingModel : PageModel
    {
        private readonly ILogger<LicensingModel> _logger;

        public LicensingModel(ILogger<LicensingModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
