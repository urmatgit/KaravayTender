// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Page
{
    public class ForgetModel : PageModel
    {
        private readonly ILogger<ForgetModel> _logger;

        public ForgetModel(ILogger<ForgetModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
