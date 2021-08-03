using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Icons
{
    public class StackShowcaseModel : PageModel
    {
        private readonly ILogger<StackShowcaseModel> _logger;

        public StackShowcaseModel(ILogger<StackShowcaseModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
