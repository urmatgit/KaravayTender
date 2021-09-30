
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MediatR;
using CleanArchitecture.Razor.Application.Features.Contragents.Commands.AddEdit;
using System;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Common.Exceptions;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using CleanArchitecture.Razor.Application.Features.Directions.Queries.GetAll;
using SmartAdmin.WebUI.Pages.Shared.Components.Contragent;

namespace SmartAdmin.WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISender _mediator;
        [BindProperty]
        public ContragentForm contragentForm { get; set; } = new();
        public RegisterModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender,
        ISender mediator
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _mediator = mediator;
        }
        

      //  public void OnGet(string returnUrl = null) => contragentForm.ReturnUrl = returnUrl;

        public async Task OnGetAsync(string returnUrl = null)
        {
            contragentForm.ReturnUrl = returnUrl;
            var request = new GetAllDirectionsQuery();
            var directionsDtos = await _mediator.Send(request);
            contragentForm.Directions = new SelectList(directionsDtos, "Id", "Name");
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
             
            //try
            //{
                var result = await _mediator.Send(contragentForm.InputContragent);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Заявка на регистрацию создана - {contragentForm.InputContragent.Name}- {contragentForm.InputContragent.ContactPhone}");
                    return LocalRedirect(returnUrl);
                }
                //else
                //{
                //    return BadRequest(Result.Failure(result.Errors));
                //}
                
            //}
            //catch (ValidationException ex)
            //{
            //    var errors = ex.Errors.Select(x => $"{ string.Join(",", x.Value) }");
            //    foreach (var error in errors)
            //    {
            //      //  ModelState.AddModelError(string.Empty, error);
            //    }

            //   // return BadRequest(Result.Failure(errors));
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError(string.Empty, ex.Message);
            //    //return BadRequest(Result.Failure(new string[] { ex.Message }));
            //}
            
            return Page();
        }
        //    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //{
        //  returnUrl = returnUrl ?? Url.Content("~/");
        //  if (ModelState.IsValid)
        //        {

        //        }
        //  //if (ModelState.IsValid)
        //  //{
        //  //  var user = new ApplicationUser { EmailConfirmed=true,
        //  //      IsActive=true,
        //  //      //Site=Input.Site,
        //  //      DisplayName= InputContragent.Name,
        //  //      UserName = InputContragent.UserName,
        //  //      Email = InputContragent.Email,
        //  //      ProfilePictureDataUrl = $"https://www.gravatar.com/avatar/{ InputContragent.Email.ToMD5() }?s=120&d=retro"
        //  //  };
        //  //  var result = await _userManager.CreateAsync(user, Input.Password);
        //  //  if (result.Succeeded)
        //  //  {
        //  //    _logger.LogInformation("User created a new account with password.");

        //  //    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //  //    //var callbackUrl = Url.Page("/Account/ConfirmEmail", null, new { userId = user.Id, code }, Request.Scheme);

        //  //    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        //  //    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //  //    await _signInManager.SignInAsync(user, false);
        //  //    return LocalRedirect(returnUrl);
        //  //  }

        //  //  foreach (var error in result.Errors)
        //  //  {
        //  //    ModelState.AddModelError(string.Empty, error.Description);
        //  //  }
        //  //}

        //  // If we got this far, something failed, redisplay form
        //  return Page();
        //}

        //public class InputModel
        //{

        //    [Display(Name = "User Name")]
        //    [Required]
        //    public string UserName { get; set; }
        //    [Required]
        //    [Display(Name = "Display Name")]
        //    public string DisplayName { get; set; }


        //    [Required]
        //    [EmailAddress]
        //    [Display(Name = "Email")]
        //    public string Email { get; set; }

        //    [Required]
        //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //    [DataType(DataType.Password)]
        //    [Display(Name = "Password")]
        //    public string Password { get; set; }

        //    [DataType(DataType.Password)]
        //    [Display(Name = "Confirm password")]
        //    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //    public string ConfirmPassword { get; set; }

        //    [Required]
        //    [Display(Name = "I agree to terms & conditions")]
        //    public bool AgreeToTerms { get; set; }

        //    [Display(Name = "Sign up for newsletters")]
        //    public bool SignUp { get; set; }
        //}
    }
}
