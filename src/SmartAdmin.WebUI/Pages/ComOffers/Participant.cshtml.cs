// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Common.Exceptions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Constants.Permission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Import;
using CleanArchitecture.Razor.Application.Features.ComOffers.Queries.Export;
using CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Delete;
using CleanArchitecture.Razor.Application.Features.ComOffers.Queries.Pagination;
using CleanArchitecture.Razor.Application.Features.ComOffers.Commands.AddEdit;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartAdmin.WebUI.Extensions;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Razor.Domain.Identity;
using CleanArchitecture.Razor.Application.Features.ComPositions.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.Categories.Queries.GetAll;
using System.Collections.Generic;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.DTOs;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.ComPositions.Queries.Pagination;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.Import;
using CleanArchitecture.Razor.Application.Features.ComStages.Commands.Create;
using CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Update;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Queries.Pagination;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Update;

namespace SmartAdmin.WebUI.Pages.ComOffers
{
    [Authorize(policy: Permissions.ComOffers.View)]
    public class ParticipantModel : PageModel
    {
        [BindProperty]
        public AddEditComOfferCommand Input { get; set; }
        [BindProperty]
        public AddEditComPositionCommand InputPos { get; set; }
        



        private readonly IIdentityService _identityService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _mediator;
        private readonly IStringLocalizer<SmartAdmin.WebUI.Pages.ComOffers.IndexModel> _localizer;
        private readonly UserManager<ApplicationUser> _userManager;

        public ParticipantModel(
           IIdentityService identityService,
            IAuthorizationService authorizationService,
            ICurrentUserService currentUserService,
            ISender mediator,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<SmartAdmin.WebUI.Pages.ComOffers.IndexModel> localizer
            )
        {
            _userManager = userManager;
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
            //Directions = await _mediator.LoadDirection();
            //var fistelement = Directions.FirstOrDefault();
            
            //    Categories = await _mediator.LoadCategory(0);
            
            //var nomenclaturies = new  GetAllNomenclaturesQuery();
            //Nomenclatures = (List<NomenclatureDto>)await  _mediator.Send(nomenclaturies);
            //Areas = await _mediator.LoadAreas();
            //await LoadManagers();

        }
       
        public async Task<IActionResult> OnGetDataAsync([FromQuery] ComOffersMyWithPaginationQuery command)
        {
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        public async Task<IActionResult> OnGetDataPosAsync([FromQuery] ComPositionsWithStagePaginationQuery command)
        {
            // throw new Exception("Test log error 222 !!!!!!");
            
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        
            public async Task<IActionResult> OnPostFailureAsync([FromBody] FailureParitipateStageCompositionCommand command)
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
        public async Task<IActionResult> OnPostPricesAsync([FromBody] UpdateStageCompositionPricesCommand command)
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
        

        public async Task<IActionResult> OnGetDeleteCheckedAsync([FromQuery] int[] id)
        {
            var command = new DeleteCheckedComOffersCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int id)
        {
            var command = new DeleteComOfferCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<FileResult> OnPostExportAsync([FromBody] ExportComOffersQuery command)
        {
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["ComOffers"] + ".xlsx");
        }
        public async Task<FileResult> OnGetCreateTemplate()
        {
            var command = new CreateComOffersTemplateCommand();
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["ComOffers"] + ".xlsx");
        }
        

         

    }
}
