// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Icons
{
    public class FontawesomeLightModel : PageModel
    {
        private readonly ILogger<FontawesomeLightModel> _logger;

        public FontawesomeLightModel(ILogger<FontawesomeLightModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
