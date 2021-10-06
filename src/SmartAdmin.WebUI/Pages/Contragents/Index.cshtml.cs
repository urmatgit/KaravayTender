using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Exceptions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
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
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [BindProperty]
        public AddEditContragentCommand Input { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        [BindProperty]
        public List<IFormFile> Files { get; set; }
        [BindProperty]
        public string CategoryIds { get; set; }
        public SelectList Directions { get; set; }
        public List<CategoryDto> Categories { get; set; } = new();

        public IndexModel(
           IIdentityService identityService,
            IAuthorizationService authorizationService,
            ICurrentUserService currentUserService,
            ISender mediator,
            IStringLocalizer<IndexModel> localizer,
               ILogger<IndexModel> logger
            )
        {
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
            var result = await _identityService.FetchUsers("Admin");


            await LoadDirection();
        }
        public async Task<IActionResult> OnGetDataAsync([FromQuery] ContragentsWithPaginationQuery command)
        {
            try
            {


                var result = await _mediator.Send(command);
                return new JsonResult(result);
            }
            catch (ValidationException ex)
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
                }
                return new JsonResult(result);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(x => $"{ string.Join(",", x.Value) }");
                return BadRequest(Result.Failure(errors));
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Failure(new string[] { ex.Message }));
            }

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
        private async Task LoadDirection()
        {
            var request = new GetAllDirectionsQuery();
            var directionsDtos = (List<DirectionDto>)await _mediator.Send(request);
            //Debug.WriteLine(JsonConvert.SerializeObject(directionsDtos));
            //Input.Directions = directionsDtos;
            Directions = new SelectList(directionsDtos, "Id", "Name");
        }
    }
}
