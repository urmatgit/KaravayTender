using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SmartAdmin.WebUI.Pages.Error
{
    public class ErrorAnnouncedModel : PageModel
    {
        private readonly ILogger<ErrorAnnouncedModel> _logger;

        public ErrorAnnouncedModel(ILogger<ErrorAnnouncedModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
