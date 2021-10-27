
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
using System.Collections.Generic;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Application.Features.Categories.Queries.GetAll;
using Microsoft.AspNetCore.Http;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.AddEdit;
using Newtonsoft.Json;
using System.Diagnostics;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.Queries.GetAll;

namespace SmartAdmin.WebUI.Pages.Contragents
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISender _mediator;
        //[BindProperty]
        //public ContragentForm contragentForm { get; set; }
        [BindProperty]
        public AddEditContragentCommand Input { get; set; }
        [BindProperty]
        public List<IFormFile> Files { get; set; }
        [BindProperty]
        public string CategoryIds { get; set; }
        public SelectList Directions { get; set; } 
        public List<CategoryDto> Categories { get; set; } = new();
        //[BindProperty]
        //    public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        
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
#if DEBUG
            Input = new AddEditContragentCommand();
            Input.Name = "name1";
            Input.FullName = "nama name2";
            Input.ContactPerson = "Person1";
            Input.ContactPhone = "+7(999)9303433";
            Input.Email = "test@gmail.com";
            Input.INN = "123456789011";
            Input.Phone= "+7(999)9303433";
            Input.DirectionId = 3;
            Input.TypeOfActivity = "Haltura";
#endif
        }


        //  public void OnGet(string returnUrl = null) => contragentForm.ReturnUrl = returnUrl;

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            await LoadDirection();
            //contragentForm.Categories = directionsDtos.SelectMany(d => d.Categories).ToList();
                                        
        }
        private async Task LoadDirection()
        {
            var request = new GetAllDirectionsQuery();
            var directionsDtos = (List<DirectionDto>)await _mediator.Send(request);
           // Debug.WriteLine(JsonConvert.SerializeObject(directionsDtos));
            //Input.Directions = directionsDtos;
            Directions = new SelectList(directionsDtos, "Id", "Name");
        }
        public async Task<PartialViewResult> OnGetCategoriesAsync([FromQuery]int directionid,int contragentid=0)
        {
            var command = new GetAllCategoriesQuery() { DirectionId = directionid };
            var result = await _mediator.Send(command);
            if (result?.Count()>0 && contragentid > 0)
            {
                var contragent = new GetByContragentCategoryQuery() { ContragentId = contragentid };
                var resultContragent = await _mediator.Send(contragent);
                if (resultContragent?.Count()>0)
                {
                    foreach (var cat in resultContragent)
                    {
                        var ResCat = result.FirstOrDefault(c => c.Id == cat.CategoryId);
                        if (ResCat != null)
                            ResCat.IsCheck = true;
                        
                    }
                }
            }
            return Partial("_CategoriesListPartial", result.ToList());// new JsonResult(result);
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Contragents/SendedRegister");
            try {
                if (Files.Count == 0)
                {
                    throw new Exception("Не добавлены файлы! ");
                }
            if (ModelState.IsValid)
            {

                 var result = await _mediator.Send(Input);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Заявка на регистрацию создана - {Input.Name}- {Input.ContactPhone}");
                    
                        var ContragetnCategory = new AddOrDelContragentCategorysCommand()
                        {
                            ContragentId = result.Data,
                            CategoriesJson = CategoryIds
                        };
                        var resultContragentCategory = await _mediator.Send(ContragetnCategory);
                        
                            if (resultContragentCategory.Succeeded)
                            {
                                _logger.LogInformation($"Категории добавлены {resultContragentCategory.Data} - {Input.Name} ");
                            }
                            else
                            {
                                foreach (var error in resultContragentCategory.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error);
                                }
                            }
                         
                    

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    //return BadRequest(Result.Failure(result.Errors));
                }
            }else
            {
                await LoadDirection();
            }

            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(x => $"{ string.Join(",", x.Value) }");
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                // return BadRequest(Result.Failure(errors));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                //return BadRequest(Result.Failure(new string[] { ex.Message }));
            }

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
        //  //      DisplayName= Input.Name,
        //  //      UserName = Input.UserName,
        //  //      Email = Input.Email,
        //  //      ProfilePictureDataUrl = $"https://www.gravatar.com/avatar/{ Input.Email.ToMD5() }?s=120&d=retro"
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
