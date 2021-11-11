// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Docs
{
    public class ProjectStructureModel : PageModel
    {
        private readonly ILogger<ProjectStructureModel> _logger;

        public ProjectStructureModel(ILogger<ProjectStructureModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
