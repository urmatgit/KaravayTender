using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Exceptions;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Application.Features.Categories.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.Contragents.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.Contragents.Commands.Delete;
using CleanArchitecture.Razor.Application.Features.Contragents.Commands.Import;
using CleanArchitecture.Razor.Application.Features.Contragents.Queries.Export;
using CleanArchitecture.Razor.Application.Features.Contragents.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.Contragents.Queries.Pagination;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.Queries.GetAll;
using CleanArchitecture.Razor.Infrastructure.Constants.Permission;
using CleanArchitecture.Razor.Infrastructure.Constants.Role;
using CleanArchitecture.Razor.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SmartAdmin.WebUI.Pages.Contragents
{
    [Authorize(policy: Permissions.Contragents.View)]
    public class IndexModel : PageModel
    {

        private readonly IIdentityService _identityService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _mediator;
        private readonly IStringLocalizer<IndexModel> _localizer;
        private readonly ILogger<IndexModel> _logger;
        private readonly  UserManager<ApplicationUser> _userManager;
        [BindProperty]
        public AddEditContragentCommand Input { get; set; }
        [BindProperty]
        public UserModel UserFormModel { get; set; } = new();

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        [BindProperty]
        public List<IFormFile> Files { get; set; }
        [BindProperty]
        public string CategoryIds { get; set; }
        public SelectList Directions { get; set; }
        public SelectList Managers { get; set; }
        public List<CategoryDto> Categories { get; set; } = new();

        public IndexModel(
           IIdentityService identityService,
            IAuthorizationService authorizationService,
            ICurrentUserService currentUserService,
            ISender mediator,
            IStringLocalizer<IndexModel> localizer,
               ILogger<IndexModel> logger,
               UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager;
            _logger = logger;
            _identityService = identityService;
            _authorizationService = authorizationService;
            _currentUserService = currentUserService;
            _mediator = mediator;
            _localizer = localizer;
            var email = _currentUserService.UserId;
        }

        public async Task OnGetAsync()
        {
            //var result = await _identityService.FetchUsers("Admin");
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            Categories = (List<CategoryDto>)result;
            //string description = Enum.GetName(typeof(CleanArchitecture.Razor.Domain.Enums.RequestStatus), -1);
            await LoadDirection();
            await LoadManagers();
        }
        public async Task<IActionResult> OnGetDataAsync([FromQuery] ContragentsWithPaginationQuery command)
        {
            try
            {


                var result = await _mediator.Send(command);
                return new JsonResult(result);
            }
            catch (CleanArchitecture.Razor.Application.Common.Exceptions.ValidationException ex)
            {
                var errors = ex.Errors.Select(x => $"{ string.Join(",", x.Value) }");
                return BadRequest(Result.Failure(errors));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(new string[] { ex.Message }));
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _mediator.Send(Input);
                    if (result.Succeeded)
                    {
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
                            return BadRequest(Result.Failure(resultContragentCategory.Errors));

                        }

                        var userApp = await CheckUser(Input.ApplicationUserId);

                        if (string.IsNullOrEmpty(userApp.Id))
                        {
                            var user = await CreateUser(userApp);
                            if (!user.Item1.Succeeded)
                            {
                                var errors = user.Item1.Errors.Select(x => $"{ string.Join(",", x.Description) }");
                                return BadRequest(errors);
                            }
                            else
                            {
                                Input.ApplicationUserId = user.Item2;
                                 result = await _mediator.Send(Input);
                            }
                        }else
                        {
                            var user = await UpdateUser(userApp, UserFormModel, Input.Email, Input.Phone);
                            if (!user.Succeeded)
                            {
                                var errors = user.Errors.Select(x => $"{ string.Join(",", x.Description) }");
                                return BadRequest(errors);
                            }
                        }
                        
                    }
                    return new JsonResult(result);
                }
                else
                {
                    return BadRequest(ModelState);

                }
            }
            catch (CleanArchitecture.Razor.Application.Common.Exceptions.ValidationException ex)
            {
                var errors = ex.Errors.Select(x => $"{ string.Join(",", x.Value) }");
                return BadRequest(Result.Failure(errors));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(new string[] { ex.Message }));
            }

        }
        private async Task<ApplicationUser> CheckUser(string id)
        {
            ApplicationUser user=null;
            if (!string.IsNullOrEmpty(id))
            {
                user = await _userManager.FindByIdAsync(id);
                
            }
            if (user==null)
            {
                user= new ApplicationUser
                {
                    EmailConfirmed = true,
                    IsActive = UserFormModel.Active,
                    PhoneNumber = UserFormModel.ManagerPhone,
                    DisplayName = Input.Name,
                    UserName = UserFormModel.Login,
                    Email = Input.Email,
                    ProfilePictureDataUrl = $"https://www.gravatar.com/avatar/{ Input.Email.ToMD5() }?s=120&d=retro"
                };
            }
            return user;
        }
        private async Task<IdentityResult> UpdateUser(ApplicationUser user, UserModel userModel,string email,string phone)
        {
            
            user.UserName = userModel.Login;
            user.PhoneNumber = phone;
            user.Email = email;
            if (!string.IsNullOrEmpty(userModel.Password))
            {
                var code =await _userManager.GeneratePasswordResetTokenAsync(user);
                var resultPass = await _userManager.ResetPasswordAsync(user, code, userModel.Password);
                if (resultPass.Succeeded)
                    _logger.LogInformation($"User changed password.({userModel.Login})");
                else
                    _logger.LogError($"User changed password.({userModel.Login})", resultPass.Errors);
            }
            var result = await _userManager.UpdateAsync(user);
            return result;
        }
        private async Task< (IdentityResult,string)> CreateUser(ApplicationUser user)
        {
             
            var result = await _userManager.CreateAsync(user, UserFormModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, new[] { RoleConstants.SupplierRole });
                _logger.LogInformation($"User created a new account with password.({UserFormModel.Login} {Input.Name})");
            }
            else
            {
                _logger.LogError($"User created error.({UserFormModel.Login} {Input.Name})", result.Errors);
                
            }
            return (result,user.Id);
        }

        public async Task<IActionResult> OnGetDeleteCheckedAsync([FromQuery] int[] id)
        {
            var command = new DeleteCheckedContragentsCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int id)
        {
            var command = new DeleteContragentCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<FileResult> OnPostExportAsync([FromBody] ExportContragentsQuery command)
        {
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["Customers"] + ".xlsx");
        }
        public async Task<FileResult> OnGetCreateTemplate()
        {
            var command = new CreateContragentsTemplateCommand();
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["Customers"] + ".xlsx");
        }
        public async Task<IActionResult> OnPostImportAsync()
        {
            var stream = new MemoryStream();
            await UploadedFile.CopyToAsync(stream);
            var command = new ImportContragentsCommand()
            {
                FileName = UploadedFile.FileName,
                Data = stream.ToArray()
            };
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        public async Task<IActionResult> OnGetManagerAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                
                UserModel userModel = new UserModel()
                {
                    Login = user.UserName,
                    ManagerPhone = user.PhoneNumber
                };
                
                return new JsonResult(userModel);
            }
            return new JsonResult("");

        }
        public async Task<IActionResult> OnGetContragentUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                
                UserModel userModel = new UserModel()
                {
                    Login = user.UserName,
                    Active = user.IsActive
                };
                return new JsonResult(userModel);
            }
            return new JsonResult("");

        }
        private async Task LoadDirection()
        {
            var request = new GetAllDirectionsQuery();
            var directionsDtos = (List<DirectionDto>)await _mediator.Send(request);
            //Debug.WriteLine(JsonConvert.SerializeObject(directionsDtos));
            //Input.Directions = directionsDtos;
            Directions = new SelectList(directionsDtos, "Id", "Name");
        }
        private async Task LoadManagers()
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            Managers = new SelectList(managers.Select(u => new { Id = u.Id, Name = string.IsNullOrEmpty(u.DisplayName) ? u.UserName : u.DisplayName }), "Id", "Name");
        }
        
        public class UserModel
        {
            
            [Display(Name = "User Name")]
            [Required(ErrorMessage = "'Логин' является обязательным")]
            public string Login { get; set; }

            [Display(Name = "Manager phone")]
            [Required(ErrorMessage = "Телефон номер менеджера не указан")]
            public string ManagerPhone { get; set; }

          //  [Required(ErrorMessage = "'Пароль' является обязательным")]
            [StringLength(20, ErrorMessage = "Пароль должен содержать не менее {2} и не более {1} символов.", MinimumLength = 6)]
            [RegularExpression("^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\\d]){1,})(?=(.*[\\W]){1,})(?!.*\\s).{6,}$", ErrorMessage = "Пароли должны состоять не менее чем из 6 символов и содержать 3 из 4 следующих символов: верхний регистр (A-Z), нижний регистр (a-z), число (0-9) и специальный символ (например !@#$%^&*)")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Пароль и пароль подтверждения не совпадают.")]
            public string ConfirmPassword { get; set; }
            public bool Active { get; set; }
                 
            
        }
    }
}
