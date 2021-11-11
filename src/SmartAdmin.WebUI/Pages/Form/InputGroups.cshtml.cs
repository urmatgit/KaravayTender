// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Form
{
    public class InputGroupsModel : PageModel
    {
        private readonly ILogger<InputGroupsModel> _logger;

        public InputGroupsModel(ILogger<InputGroupsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
