// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Miscellaneous
{
    public class TreeviewModel : PageModel
    {
        private readonly ILogger<TreeviewModel> _logger;

        public TreeviewModel(ILogger<TreeviewModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
