// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Plugins
{
    public class WavesModel : PageModel
    {
        private readonly ILogger<WavesModel> _logger;

        public WavesModel(ILogger<WavesModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
