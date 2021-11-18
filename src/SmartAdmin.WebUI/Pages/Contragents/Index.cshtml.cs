// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
using CleanArchitecture.Razor.Application.Features.Contragents.Commands.Update;
using CleanArchitecture.Razor.Application.Features.Contragents.Queries.Export;
using CleanArchitecture.Razor.Application.Features.Contragents.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.Contragents.Queries.GetCount;
using CleanArchitecture.Razor.Application.Features.Contragents.Queries.Pagination;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.StatusLogs.Queries.Pagination;
using CleanArchitecture.Razor.Domain.Constants;
using CleanArchitecture.Razor.Domain.Enums;

using CleanArchitecture.Razor.Infrastructure.Constants.Permission;
using CleanArchitecture.Razor.Infrastructure.Constants.Role;
using CleanArchitecture.Razor.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUploadService _uploadService;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public AddEditContragentCommand Input { get; set; }
        [BindProperty]
        public UserModel UserFormModel { get; set; } = new();

        [BindProperty]
        public IFormFile UploadedFile { get; set; }
        [BindProperty]
        public RejectFormModel RejectModel { get; set; }

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
               UserManager<ApplicationUser> userManager,
                IUploadService uploadService,
                IEmailSender emailSender
            )
        {
            _emailSender = emailSender;
            _uploadService = uploadService;
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
        public async Task<IActionResult> OnGetFilesListAsync(int id)
        {
            try
            {
                var files = await _uploadService.LoadFilesAsync(id, PathConstants.DocumentsPath);
                return new JsonResult(files?.Data);
            }
            catch (Exception er)
            {
                return BadRequest(new string[] { er.Message });
            }
        }
        public async Task<IActionResult> OnGetDeleteFileAsync([FromQuery] int id, [FromQuery] string name)
        {
            var _canDeleteFile = await _authorizationService.AuthorizeAsync(User, null, Permissions.Contragents.DeleteFile);
            if (_canDeleteFile.Succeeded)
            {
                var result = _uploadService.RemoveFileAsync(id, name, PathConstants.DocumentsPath);

                return new JsonResult(_localizer["Delete Success"]);
            }
            else
                return BadRequest(Result.Failure(new string[] { "У вас нет прав на удаление файла!" }));

        }
        public async Task<IActionResult> OnGetOnRegistraionCountAsync()
        {
            var count = await _mediator.Send(new GetByStatusQuery() { Status = ContragentStatus.OnRegistration });
            return new JsonResult(count);
        }
        public async Task<IActionResult> OnGetStatusLogsAsync([FromQuery] StatusLogsWithPaginationQuery command)
        {
            try
            {

                if (command.ContragentId is null || command.ContragentId == 0)
                    return new JsonResult("");

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
                //if (Request.Form.Files.Count == 0)
                //{
                //    throw new Exception("Не добавлены файлы! ");
                //}
                if (ModelState.IsValid)
                {
                    if (Input.Id == 0) //When adding new contragent, check exist contragent by key fields
                    {
                        var checking = new CheckExistByParamsQuery
                        {
                            Email = Input.Email,
                            INN = Input.INN,
                            Name = Input.Name
                        };
                        var checkExist = await _mediator.Send(checking);
                        if (checkExist.Data != null)
                        {
                            throw new Exception($"Контрагент c такими ключевыми параметрами уже существует  ('{Input.Email}' '{Input.INN}' '{Input.Name}')!"); ;
                        }
                    }

                    //create app user
                    var userApp = await CheckUser(Input.ApplicationUserId);

                    if (!userApp.Item2)
                    {
                        if (string.IsNullOrEmpty(UserFormModel.Password))
                        {

                            return BadRequest("Пароль не задан!!!");
                        }
                        var user = await CreateUser(userApp.Item1);
                        if (!user.Item1.Succeeded)
                        {
                            var errors = user.Item1.Errors.Select(x => $"{ string.Join(",", x.Description) }");
                            return BadRequest(errors);
                        }
                        else
                        {
                            Input.ApplicationUserId = user.Item2;

                        }
                    }
                    else
                    {
                        var user = await UpdateUser(userApp.Item1, UserFormModel, Input.Email, Input.Phone);
                        if (!user.Succeeded)
                        {
                            var errors = user.Errors.Select(x => $"{ string.Join(",", x.Description) }");
                            return BadRequest(errors);
                        }
                    }
                    if (UserFormModel.Active)
                        Input.Status = ContragentStatus.Registered;
                    else
                        Input.Status = ContragentStatus.NotActive;
                    _logger.LogInformation($"Contragent {Input.Name} set status {Input.Status}");

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




                        if (Request?.Form?.Files?.Count > 0)
                        {
                            await _uploadService.UploadFileAsync(result.Data,PathConstants.DocumentsPath,  Request.Form.Files.ToList());
                        }

                        await SendMessageToCustomer();
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
                _logger.LogError(ex.Message, ex);
                return BadRequest(errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(new string[] { ex.Message });
            }

        }
        private async Task SendMessageToCustomer()
        {
            var StatusStr = "";
            switch (Input.Status)
            {
                case ContragentStatus.Registered:
                    StatusStr = $"Зарегистрирован.\n Ваш логин: {UserFormModel.Login},   пароль: {UserFormModel.Password} \n Войдите в личный кабинет, перейдя по следующей ссылке: https://localhost:44340/Identity/Account/Login";
                    break;
                case ContragentStatus.Reject:
                    StatusStr = "Отклонена";
                    break;
                default:
                    return;
            }
            var messageBody = $"Уважаемый поставщик  Ваша заявка на регистрацию от {Input.Created} числа, {StatusStr}";
            await _emailSender.SendEmailAsync("", "Ответ заявку на регистрацию", messageBody);
        }
        private async Task<(ApplicationUser, bool)> CheckUser(string id)
        {
            ApplicationUser user = null;
            bool IsExist = false;
            if (!string.IsNullOrEmpty(id))
            {
                user = await _userManager.FindByIdAsync(id);
                IsExist = user != null;
            }
            if (user == null)
            {

                user = new ApplicationUser
                {
                    EmailConfirmed = true,
                    IsActive = UserFormModel.Active,
                    PhoneNumber = Input.Phone,
                    DisplayName = Input.Name,
                    UserName = UserFormModel.Login,
                    Email = Input.Email,
                    ProfilePictureDataUrl = $"https://www.gravatar.com/avatar/{ Input.Email.ToMD5() }?s=120&d=retro"
                };
            }
            return (user, IsExist);
        }
        private async Task<IdentityResult> UpdateUser(ApplicationUser user, UserModel userModel, string email, string phone)
        {

            var checkEmail = await _userManager.FindByEmailAsync(email);
            if (checkEmail != null && checkEmail.UserName != userModel.Login)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"{email} уже используется!"
                });
            }
            user.UserName = userModel.Login;
            user.PhoneNumber = phone;
            user.Email = email;
            user.IsActive = userModel.Active;
            if (!string.IsNullOrEmpty(userModel.Password))
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resultPass = await _userManager.ResetPasswordAsync(user, code, userModel.Password);
                if (resultPass.Succeeded)
                    _logger.LogInformation($"User changed password.({userModel.Login})");
                else
                    _logger.LogError($"User changed password.({userModel.Login})", resultPass.Errors);
            }

            var result = await _userManager.UpdateAsync(user);
            return result;
        }
        private async Task<(IdentityResult, string)> CreateUser(ApplicationUser user)
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
            return (result, user.Id);
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
        public async Task<IActionResult> OnPostRejectAsync([FromBody] RejectFormModel model)
        {

            if (Input.Status != ContragentStatus.OnRegistration)
                return BadRequest("Отклонение только для статуса 'На регистрации'");
            var command = new UpdateStatusContragentCommand()
            {
                Id = model.Id,
                Status = ContragentStatus.Reject,
                Description = model.Description
            };
            var result = await _mediator.Send(command);
            if (!result.Succeeded)
            {
                _logger.LogError($"Сontragent reject error.({model.Id} {ContragentStatus.Reject})", result.Errors);
                return BadRequest(result.Errors);
            }
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

        public class RejectFormModel
        {

            public string Description { get; set; }
            public int Id { get; set; }
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
            public bool Active { get; set; } = true;


        }
    }
}
