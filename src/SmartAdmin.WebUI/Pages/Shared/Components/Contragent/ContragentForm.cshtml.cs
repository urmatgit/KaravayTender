using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Features.Contragents.Commands.AddEdit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartAdmin.WebUI.Pages.Shared.Components.Contragent
{
    public class ContragentForm
    {
        [BindProperty]
        public AddEditContragentCommand InputContragent { get; set; }
        public SelectList Directions { get; set; }
        //[BindProperty]
        //    public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
    }
}
