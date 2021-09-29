using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CleanArchitecture.Razor.Infrastructure.Constants.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MediatR;
using CleanArchitecture.Razor.Application.Features.Categories.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using Microsoft.Extensions.Localization;
using CleanArchitecture.Razor.Application.Features.Categories.Queries.Pagination;

using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Categories.Commands.Delete;
using CleanArchitecture.Razor.Application.Features.Categories.Queries.Export;
using System.IO;
using CleanArchitecture.Razor.Application.Features.Categories.Commands.Import;
using CleanArchitecture.Razor.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Rendering;
using CleanArchitecture.Razor.Application.Features.Directions.Queries.GetAll;

namespace SmartAdmin.WebUI.Pages.Categories
{
    [Authorize(policy: Permissions.Categories.View)]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public AddEditCategoryCommand Input { get; set; }
        
        
        public SelectList Directions { get; set; }
        public string[] filterDirection = new string[]
        {
            "фильтр1","фильтр2","фильтр3","фильтр4"
        };
        private readonly IIdentityService _identityService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _mediator;
        private readonly IStringLocalizer<IndexModel> _localizer;

        public IndexModel(
           IIdentityService identityService,
            IAuthorizationService authorizationService,
            ICurrentUserService currentUserService,
            ISender mediator,
            IStringLocalizer<IndexModel> localizer
            )
        {
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
            var request = new GetAllDirectionsQuery();
            var directionsDtos = await _mediator.Send(request);
            Directions = new SelectList(directionsDtos, "Id", "Name");
        }
        public async Task<IActionResult> OnGetDataAsync([FromQuery] CategoriesWithPaginationQuery command)
        {
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var result = await _mediator.Send(Input);
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
            var command = new DeleteCheckedCategoriesCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int id)
        {
            var command = new DeleteCategoryCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<FileResult> OnPostExportAsync([FromBody] ExportCategoriesQuery command)
        {
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["Categories"] + ".xlsx");
        }
        public async Task<FileResult> OnGetCreateTemplate()
        {
            var command = new CreateCategoriesTemplateCommand();
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["Categories"] + ".xlsx");
        }
        //public async Task<IActionResult> OnPostImportAsync()
        //{
        //    var stream = new MemoryStream();
        //    await UploadedFile.CopyToAsync(stream);
        //    var command = new ImportCategoriesCommand()
        //    {
        //        FileName = UploadedFile.FileName,
        //        Data = stream.ToArray()
        //    };
        //    var result = await _mediator.Send(command);
        //    return new JsonResult(result);
        //}

    }
}
