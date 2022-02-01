// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Statistics
{
    public class FlotModel : PageModel
    {
        private readonly ILogger<FlotModel> _logger;

        public FlotModel(ILogger<FlotModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
