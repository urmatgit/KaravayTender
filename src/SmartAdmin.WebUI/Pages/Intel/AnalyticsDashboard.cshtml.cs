// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Intel
{
    public class AnalyticsDashboardModel : PageModel
    {
        private readonly ILogger<AnalyticsDashboardModel> _logger;

        public AnalyticsDashboardModel(ILogger<AnalyticsDashboardModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
