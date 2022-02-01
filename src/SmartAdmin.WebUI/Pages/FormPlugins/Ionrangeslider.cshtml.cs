// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.FormPlugins
{
    public class IonrangesliderModel : PageModel
    {
        private readonly ILogger<IonrangesliderModel> _logger;

        public IonrangesliderModel(ILogger<IonrangesliderModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
