using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
