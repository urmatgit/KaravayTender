// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Documentation
{
    public class HowtoContributeModel : PageModel
    {
        private readonly ILogger<HowtoContributeModel> _logger;

        public HowtoContributeModel(ILogger<HowtoContributeModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
