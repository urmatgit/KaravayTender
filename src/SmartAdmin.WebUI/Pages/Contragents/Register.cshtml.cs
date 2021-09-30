using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Features.Contragents.Commands.AddEdit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartAdmin.WebUI.Pages.Contragents
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public AddEditContragentCommand InputContragent { get; set; }
        public void OnGet()
        {
        }
    }
}
