// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Behaviours;
using CleanArchitecture.Razor.Application.Common.Exceptions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Features.Categories.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.Delete;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.Import;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Queries.Export;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Queries.Pagination;
using CleanArchitecture.Razor.Domain.Constants;

using CleanArchitecture.Razor.Application.Constants.Permission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using SmartAdmin.WebUI.Extensions;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.Update;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Queries.GetAll;
using Newtonsoft.Json;

namespace SmartAdmin.WebUI.Pages.Nomenclatures
{
    [Authorize(policy: Permissions.Nomenclatures.View)]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public AddEditNomenclatureCommand Input { get; set; }
        [BindProperty]
        public IFormFile UploadedFile { get; set; }
        [Required(ErrorMessage = "Загрузите спецификации!")]
        [BindProperty]
        public List<IFormFile> Files { get; set; }

        public SelectList Directions { get; set; }
        public SelectList Categories { get; set; }
        public SelectList UnitOfs { get; set; }
        public SelectList Vats { get; set; }
        public SelectList QualityDocs { get; set; }

        private readonly IIdentityService _identityService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _mediator;
        private readonly IStringLocalizer<IndexModel> _localizer;
        private readonly IUploadService _uploadService;

        public IndexModel(
           IIdentityService identityService,
            IAuthorizationService authorizationService,
            ICurrentUserService currentUserService,
            IUploadService uploadService,
            ISender mediator,
            IStringLocalizer<IndexModel> localizer
            )
        {
            _uploadService = uploadService;
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
            Directions = await _mediator.LoadDirection();
            var fistelement = Directions.FirstOrDefault();
            if (fistelement != null) {
                Categories = await _mediator.LoadCategory(Convert.ToInt32(fistelement.Value));
            }
            UnitOfs = await _mediator.LoadUnitOf();
            Vats = await _mediator.LoadVats();
            QualityDocs = await _mediator.LoadQualityDocs();
        }
        public async Task<IActionResult> OnGetById([FromQuery]int id)
        {
            // throw new Exception("Test log error 222 !!!!!!");

            var result = await _mediator.Send(new GetByIdNomenclatureQuery { Id=id});
            //var  jsonSerializerSettings = new System.Text.Json.JsonSerializerOptions();
            //jsonSerializerSettings.MaxDepth = 3;
            var resultJson= new JsonResult(result );
            return resultJson;
        }
        public async Task<IActionResult> OnGetDataAsync([FromQuery] NomenclaturesWithPaginationQuery command)
        {
           // throw new Exception("Test log error 222 !!!!!!");
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                
                var result = await _mediator.Send(Input);
                if (result.Succeeded && Files.Count > 0)
                {
                    var resultUpload= await _uploadService.UploadFileAsync(result.Data, PathConstants.SpecificationsPath, Files);
                    if (resultUpload.Succeeded)
                    {
                        var files = await _uploadService.LoadFilesAsync(result.Data, PathConstants.SpecificationsPath);
                        if (files.Succeeded)
                        {
                            Input.Specifications = string.Join(PathConstants.FilesStringSeperator, files.Data.Select(f => Path.GetFileName(f)));
                            if (Input.Id==0)
                                Input.Id = result.Data;
                            result = await _mediator.Send(Input);
                        }
                    }
                }
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

        public async Task<IActionResult> OnGetDeleteCheckedAsync([FromQuery] int[] id)
        {
            var command = new DeleteCheckedNomenclaturesCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int id)
        {
            var command = new DeleteNomenclatureCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<FileResult> OnPostExportAsync([FromBody] ExportNomenclaturesQuery command)
        {
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["Nomenclatures"] + ".xlsx");
        }
        public async Task<FileResult> OnGetCreateTemplate()
        {
            var command = new CreateNomenclaturesTemplateCommand();
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["Nomenclatures"] + ".xlsx");
        }
        public async Task<IActionResult> OnPostImportAsync()
        {
            var stream = new MemoryStream();
            await UploadedFile.CopyToAsync(stream);
            var command = new ImportNomenclaturesCommand()
            {
                FileName = UploadedFile.FileName,
                Data = stream.ToArray()
            };
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        public async Task<IActionResult> OnGetDeleteFileAsync([FromQuery] int id, [FromQuery] string name)
        {
            var _canDeleteFile= await _authorizationService.AuthorizeAsync(User, null, Permissions.Nomenclatures.DeleteFile);
            if (_canDeleteFile.Succeeded)
            {
                var result= await _uploadService.RemoveFileAsync(id, name, PathConstants.SpecificationsPath);
                if (result.Succeeded)
                {
                    result = await _mediator.Send(new UpdateSpecificationsNomenclatureCommand { Id = id });
                }
                return new JsonResult(_localizer["Delete Success"]);
            }
            else
                return BadRequest(Result.Failure(new string[] { "У вас нет прав на удаление файла!" }));
            
        }
        public async Task<IActionResult> OnGetFilesListAsync(int id)
        {
            try
            {
                var files = await _uploadService.LoadFilesAsync(id, PathConstants.SpecificationsPath);
                return new JsonResult(files?.Data);
            }
            catch (Exception er)
            {
                return BadRequest(new string[] { er.Message });
            }
        }

        #region other
        public async Task<IActionResult> OnGetCategoriesAsync(int directionid)
        {
            var command = new GetAllCategoriesQuery() { DirectionId = directionid };
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

        #endregion
    }
}
