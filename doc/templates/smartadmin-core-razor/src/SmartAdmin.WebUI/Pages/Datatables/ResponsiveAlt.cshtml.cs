using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Datatables
{
    public class ResponsiveAltModel : PageModel
    {
        private readonly ILogger<ResponsiveAltModel> _logger;

        public ResponsiveAltModel(ILogger<ResponsiveAltModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
