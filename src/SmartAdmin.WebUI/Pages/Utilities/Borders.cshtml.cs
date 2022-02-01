// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Utilities
{
    public class BordersModel : PageModel
    {
        private readonly ILogger<BordersModel> _logger;

        public BordersModel(ILogger<BordersModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
