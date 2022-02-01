// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Exceptions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Customers.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Products.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Products.Commands.Delete;
using CleanArchitecture.Razor.Application.Products.Commands.Import;
using CleanArchitecture.Razor.Application.Products.Queries.Export;
using CleanArchitecture.Razor.Application.Products.Queries.Pagination;
using CleanArchitecture.Razor.Infrastructure.Constants.Permission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace SmartAdmin.WebUI.Pages.DemoMasterDetail
{
    [Authorize(policy: Permissions.Products.View)]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public AddEditProductCommand Input { get; set; }
        [BindProperty]
        public AddEditCustomerCommand InputCustomer { get; set; }
        [BindProperty]
        public IFormFile UploadedFile { get; set; }

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
            var result = await _identityService.FetchUsers("Admin");
        }
        public async Task<IActionResult> OnGetDataAsync([FromQuery] ProductsWithPaginationQuery command)  // CustomersWithPaginationQuery command)
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
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(string.Join(",", errors));
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(ex.Message);
            }
        }

        public async Task<IActionResult> OnGetDeleteCheckedAsync([FromQuery] int[] id)
        {
            var command = new DeleteCheckedProductsCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int id)
        {
            var command = new DeleteProductCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<FileResult> OnPostExportAsync([FromBody] ExportProductsQuery command)
        {
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["Products"] + ".xlsx");
        }
        public async Task<FileResult> OnGetCreateTemplate()
        {
            var command = new CreateProductTemplateCommand();
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["Products"] + ".xlsx");
        }
        public async Task<IActionResult> OnPostImportAsync()
        {
            var stream = new MemoryStream();
            await UploadedFile.CopyToAsync(stream);
            var command = new ImportProductCommand()
            {
                FileName = UploadedFile.FileName,
                Data = stream.ToArray()
            };
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }

    }
}
