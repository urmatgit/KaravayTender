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
using CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.Delete;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.Import;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Queries.Export;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Queries.Pagination;
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
using CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.Update;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.DTOs;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Queries.GetAll;

namespace SmartAdmin.WebUI.Pages.ComParticipants
{
    [Authorize(policy: Permissions.ComOffers.View)]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public AddEditComParticipantCommand InputPar { get; set; }
        [BindProperty]
        public AddContragentsComParticipantCommand InputContrPar { get; set; }





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
           
        }
        //public async Task<IActionResult> OnGetById([FromQuery]int id)
        //{
        //    // throw new Exception("Test log error 222 !!!!!!");

        //    var result = await _mediator.Send(new GetByIdComParticipantQuery { Id=id});
        //    return new JsonResult(result);
        //}
        public async Task<IActionResult> OnGetDataAsync([FromQuery] ComParticipantsWithPaginationQuery command)
        {
           // throw new Exception("Test log error 222 !!!!!!");
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        public async Task<IActionResult> OnGetActiveParticipantsAsync([FromQuery] GetParticipantsLastWithQuery command)
        {
            // throw new Exception("Test log error 222 !!!!!!");
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                var result = await _mediator.Send(InputPar);

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
        public async Task<IActionResult> OnPostAddMassAsync([FromBody] AddContragentsComParticipantCommand command)
        {
            try
            {

                var result = await _mediator.Send(InputContrPar);
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
        public async Task<IActionResult> OnGetDeleteCheckedAsync([FromQuery] int[] id,int comofferid)
        {
            var command = new DeleteCheckedComParticipantsCommand() { Id = id,ComOfferId=comofferid };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int  contragentId, int comofferid)
        {
            var command = new DeleteComParticipantCommand() { ContragentId= contragentId, ComOfferId= comofferid };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<FileResult> OnPostExportAsync([FromBody] ExportComParticipantsQuery command)
        {
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["ComParticipants"] + ".xlsx");
        }
        public async Task<FileResult> OnGetCreateTemplate()
        {
            var command = new CreateComParticipantsTemplateCommand();
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["ComParticipants"] + ".xlsx");
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
        
        #endregion
    }
}
