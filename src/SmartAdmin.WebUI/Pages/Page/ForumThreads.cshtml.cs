// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Page
{
    public class ForumThreadsModel : PageModel
    {
        private readonly ILogger<ForumThreadsModel> _logger;

        public ForumThreadsModel(ILogger<ForumThreadsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
