// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Datatables
{
    public class FixedcolumnsModel : PageModel
    {
        private readonly ILogger<FixedcolumnsModel> _logger;

        public FixedcolumnsModel(ILogger<FixedcolumnsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
