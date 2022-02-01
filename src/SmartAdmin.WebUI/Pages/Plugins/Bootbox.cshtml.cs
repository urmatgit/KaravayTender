// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Plugins
{
    public class BootboxModel : PageModel
    {
        private readonly ILogger<BootboxModel> _logger;

        public BootboxModel(ILogger<BootboxModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
