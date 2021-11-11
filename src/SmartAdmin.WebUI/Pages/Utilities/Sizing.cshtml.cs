// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Utilities
{
    public class SizingModel : PageModel
    {
        private readonly ILogger<SizingModel> _logger;

        public SizingModel(ILogger<SizingModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
