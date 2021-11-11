// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Utilities
{
    public class PositionModel : PageModel
    {
        private readonly ILogger<PositionModel> _logger;

        public PositionModel(ILogger<PositionModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
