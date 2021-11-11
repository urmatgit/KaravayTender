// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Statistics
{
    public class DygraphModel : PageModel
    {
        private readonly ILogger<DygraphModel> _logger;

        public DygraphModel(ILogger<DygraphModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
