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
using CleanArchitecture.Razor.Application.Features.ComStages.Commands.Update;
using CleanArchitecture.Razor.Application.Features.StageCompositions.Commands.Update;

namespace SmartAdmin.WebUI.Pages.ComOffers
{
    [Authorize(policy: Permissions.ComOffers.Manage)]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public AddEditComOfferCommand Input { get; set; }
        [BindProperty]
        public AddEditComPositionCommand InputPos { get; set; }

        [BindProperty]
        public AddEditComParticipantCommand InputPar { get; set; }
        [BindProperty]
        public AddContragentsComParticipantCommand InputContrPar { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }
        public SelectList Directions { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Managers { get; set; }
        public SelectList Areas { get; set; }
        public List<NomenclatureDto> Nomenclatures { get; private set; } = new List<NomenclatureDto>();

        private readonly IIdentityService _identityService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _mediator;
        private readonly IStringLocalizer<IndexModel> _localizer;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
           IIdentityService identityService,
            IAuthorizationService authorizationService,
            ICurrentUserService currentUserService,
            ISender mediator,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<IndexModel> localizer
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
            var result = await _identityService.FetchUsers("Admin");
            Directions = await _mediator.LoadDirection();
            var fistelement = Directions.FirstOrDefault();
            
              //  Categories = await _mediator.LoadCategory(0);
            
            //var nomenclaturies = new  GetAllNomenclaturesQuery();
            //Nomenclatures = (List<NomenclatureDto>)await  _mediator.Send(nomenclaturies);
            Areas = await _mediator.LoadAreas();
            await LoadManagers();

        }
        private async Task LoadManagers()
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            Managers = new SelectList(managers.Select(u => new { Id = u.Id, Name = string.IsNullOrEmpty(u.DisplayName) ? u.UserName : u.DisplayName }), "Id", "Name");
        }
        public async Task<IActionResult> OnGetDataAsync([FromQuery] ComOffersWithPaginationQuery command)
        {
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        public async Task<IActionResult> OnGetDataPosAsync([FromQuery] ComPositionsWithPaginationQuery command)
        {
            // throw new Exception("Test log error 222 !!!!!!");
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        public async Task<IActionResult> OnPostCopyAsync([FromQuery]  int id)
        {
            try
            {
                
                var result = await _mediator.Send(new CopyComOfferCommand {Id=id });
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
                if (Input.Status != CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Preparation)
                    return BadRequest("Коммерческое предложения уже запущена!!! ");
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
        public async Task<IActionResult> OnPostCancelComOfferAsync()
        {
            try
            {
                if (Input.Status != CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Evaluation)
                    return BadRequest(Result.Failure(new string[] { "Статус не соответствует для этой операции!" }));
                
                var resultComOffer = await _mediator.Send(new UpdateStatusComOfferCommand() { Id = Input.Id, Status = CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Cancelled });
                return new JsonResult(resultComOffer);

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
        public async Task<IActionResult> OnPostSelectWinnerAsync([FromBody] NextComStageWinnerCommand command)
        {
            try
            {
                //if (Input.Status != CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Evaluation)
                //    return BadRequest(Result.Failure(new string[] { "Статус не соответствует для этой операции!" }));
                

                var result = await _mediator.Send(command);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);
                //return new JsonResult("");
                var resultComOffer = await _mediator.Send(new UpdateStatusComOfferCommand() { Id = command.ComOfferId, Status = CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.WinnerDetermining });
                return new JsonResult(resultComOffer);

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
        public async Task<IActionResult> OnPostEndStageAsync([FromQuery] int stageid)
        {
            try
            {
                if (Input.Status != CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Waiting)
                    return BadRequest(Result.Failure(new string[] { "Статус не соответствует для этой операции!" }));
                var resultComOffer = await _mediator.Send(new UpdateStatusComOfferCommand() { Id = Input.Id, Status = CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Evaluation });
                //TODO снят галочки у запроса цен
                var uncheck = await _mediator.Send(new UncheckPriceRequesStageCompositionCommand() { ComOfferId = Input.Id });
                if (!uncheck.Succeeded)
                    return BadRequest(uncheck); 

                return new JsonResult(resultComOffer);
                
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
        public async Task<IActionResult> OnPostSendPriceAsync([FromBody] UpdateStageCompositionPricesManagerCommand command)
        {
            try
            {
                //if (Input.Status != CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Evaluation)
                //    return BadRequest(Result.Failure(new string[] { "Статус КП не соответствует для отправки запроса!" })) ;
                var result = await _mediator.Send(command);
                if (result.Succeeded)
                {
                    var resultComOffer = await _mediator.Send(new UpdateStatusComOfferCommand() { Id = command.stageComRequest.ComOfferId, Status = CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Waiting });
                    return new JsonResult(resultComOffer);
                }
                else
                    return BadRequest(result.Errors);
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
        public async Task<IActionResult> OnPostDeadlineAsync([FromQuery] DateTime deadline, int stageId)
        {
            if (Input.Status != CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Waiting)
                return BadRequest("Изменение  'срок ответа до' можно только в статусе 'Ожидание КП' ");
            var result = await _mediator.Send(new UpdateDeadlineComStageCommand() { Id = stageId, DeadLine = deadline });

            if (result.Succeeded)
            {
                return new JsonResult("");
            }else
            {
                return BadRequest(result);
            }
        }
        public async Task<IActionResult> OnPostRunAsync([FromQuery] DateTime deadline)
        {
            try
            {
                if (Input.Status != CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Preparation)
                    return BadRequest("Коммерческое предложения уже запущена!!! ");
                var resultInput = await _mediator.Send(Input);
                if (resultInput.Succeeded)
                {

                    var CreateState1 = new CreateComStageCommand()
                    {
                        ComOfferId = Input.Id,
                        Number = 1,
                        DeadlineDate = deadline,

                    };
                    var result = await _mediator.Send(CreateState1);
                    if (!result.Succeeded)
                    {
                        return BadRequest(Result.Failure(result.Errors));

                    }
                    var resultComOffer = await _mediator.Send(new UpdateStatusComOfferCommand() { Id = Input.Id, Status = CleanArchitecture.Razor.Domain.Enums.ComOfferStatus.Waiting });
                    return new JsonResult(resultComOffer);
                }
                else
                    return BadRequest(resultInput.Errors);
                //return new JsonResult("OK");
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
        public async Task<IActionResult> OnPostImportAsync()
        {
            var stream = new MemoryStream();
            await UploadedFile.CopyToAsync(stream);
            var command = new ImportComOffersCommand()
            {
                FileName = UploadedFile.FileName,
                Data = stream.ToArray()
            };
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetCategoriesAsync(int directionid)
        {
            var command = new GetAllCategoriesQuery() { DirectionId = directionid };
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }

    }
}
