// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Ui
{
    public class ProgressBarsModel : PageModel
    {
        private readonly ILogger<ProgressBarsModel> _logger;

        public ProgressBarsModel(ILogger<ProgressBarsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
