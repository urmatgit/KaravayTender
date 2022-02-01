// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.FormPlugins
{
    public class Select2Model : PageModel
    {
        private readonly ILogger<Select2Model> _logger;

        public Select2Model(ILogger<Select2Model> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
