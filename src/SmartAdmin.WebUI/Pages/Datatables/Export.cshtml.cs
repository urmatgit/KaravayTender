// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Datatables
{
    public class ExportModel : PageModel
    {
        private readonly ILogger<ExportModel> _logger;

        public ExportModel(ILogger<ExportModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
