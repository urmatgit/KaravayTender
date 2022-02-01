// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Documentation
{
    public class GettingStartedModel : PageModel
    {
        private readonly ILogger<GettingStartedModel> _logger;

        public GettingStartedModel(ILogger<GettingStartedModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
