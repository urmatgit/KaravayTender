// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Statistics
{
    public class C3Model : PageModel
    {
        private readonly ILogger<C3Model> _logger;

        public C3Model(ILogger<C3Model> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
