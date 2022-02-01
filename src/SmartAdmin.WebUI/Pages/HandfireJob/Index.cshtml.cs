using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CleanArchitecture.Razor.Application.Constants.Permission;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartAdmin.WebUI.Pages.HandfireJob
{
    [Authorize(policy: Permissions.Hangfire.View)]
    public class IndexModel : PageModel
    {
        public async Task OnGetAsync()
        {
            var _accessToken = await HttpContext.GetTokenAsync("access_token");
           
            Response.Redirect($"/Jobs?token={_accessToken}");
        }
    }
}
