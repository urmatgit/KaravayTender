// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Page
{
    public class ForumDiscussionModel : PageModel
    {
        private readonly ILogger<ForumDiscussionModel> _logger;

        public ForumDiscussionModel(ILogger<ForumDiscussionModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
