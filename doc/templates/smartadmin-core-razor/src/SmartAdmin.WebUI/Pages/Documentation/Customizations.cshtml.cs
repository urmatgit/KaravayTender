using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Documentation
{
    public class CustomizationsModel : PageModel
    {
        private readonly ILogger<CustomizationsModel> _logger;

        public CustomizationsModel(ILogger<CustomizationsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
