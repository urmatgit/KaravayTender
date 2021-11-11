// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Plugins
{
    public class PacejsModel : PageModel
    {
        private readonly ILogger<PacejsModel> _logger;

        public PacejsModel(ILogger<PacejsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
