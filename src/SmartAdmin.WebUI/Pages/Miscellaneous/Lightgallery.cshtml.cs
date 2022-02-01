// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Miscellaneous
{
    public class LightgalleryModel : PageModel
    {
        private readonly ILogger<LightgalleryModel> _logger;

        public LightgalleryModel(ILogger<LightgalleryModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
