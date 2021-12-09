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
using CleanArchitecture.Razor.Application.Features.ComStages.Commands.AddEdit;
using CleanArchitecture.Razor.Application.Features.ComStages.Commands.Delete;
using CleanArchitecture.Razor.Application.Features.ComStages.Commands.Import;
using CleanArchitecture.Razor.Application.Features.ComStages.Queries.Export;
using CleanArchitecture.Razor.Application.Features.ComStages.Queries.Pagination;
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
using CleanArchitecture.Razor.Application.Features.ComStages.Commands.Update;
using CleanArchitecture.Razor.Application.Features.ComStages.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.DTOs;
using CleanArchitecture.Razor.Application.Features.Nomenclatures.Queries.GetAll;

using CleanArchitecture.Razor.Application.Features.ComStages.Queries.GetBy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Features.ComStages.DTOs;
using CleanArchitecture.Razor.Application.Features.Contragents.DTOs;
using CleanArchitecture.Razor.Application.Features.ComPositions.DTOs;
using CleanArchitecture.Razor.Application.Features.ComOffers.DTOs;
using CleanArchitecture.Razor.Application.Features.ComParticipants.DTOs;
using CleanArchitecture.Razor.Domain.Enums;
using CleanArchitecture.Razor.Application.Features.ComStages.Queries.GetCross;

namespace SmartAdmin.WebUI.Pages.ComStages
{
    [Authorize(policy: Permissions.ComOffers.View)]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public AddEditComStageCommand InputPos { get; set; }
        
        [BindProperty]
        public int TypeOffer { get; set; }


        

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
        public async Task<IActionResult> OnGetById([FromQuery] int stage, [FromQuery] int comid)
        {
            // throw new Exception("Test log error 222 !!!!!!");

            var result = await _mediator.Send(new GetByStageQuery { Stage = 1, ComOfferId = 1 });
            var Nomenclatures = result.StageCompositions.GroupBy(c => c.ComPosition).Select(c => new
            {
                ComPosition = c.Key,
                participients = c.ToList(),

            });
            //var NomenclaturesDic = result.StageCompositions.GroupBy(c => c.ComPosition).Select(c => new
            //{
            //    ComPosition = c.Key,

            //    participients = c.ToDictionary(c => c.Contragent, c => c)

            //});

            //var setting = new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //};
            //var json = JsonConvert.SerializeObject(NomenclaturesDic, setting);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var nomenclature in Nomenclatures)
            {
                stringBuilder.Append('{');
                stringBuilder.Append($"StageNumber:{stage},");
                stringBuilder.Append($"ComPositionId:{nomenclature.ComPosition.Id},");
                stringBuilder.Append($"NomName:'{nomenclature.ComPosition.Nomenclature.Name}',");
                 StringBuilder stringBuilderPart = new StringBuilder();
                foreach (var part in nomenclature.participients)
                {
                    stringBuilderPart.Append($"'{part.Contragent.Name} ({part.ComPosition.ComOffer.ComParticipants.FirstOrDefault(p=>p.ContragentId==part.ContragentId)?.Status.ToDescriptionString()})':{part.Contragent.Id},");
                    stringBuilderPart.Append($"'ContName_{part.Contragent.Id}_Status':'{part.Status}',");
                    stringBuilderPart.Append($"'ContName_{part.Contragent.Id}_Price':'{part.Price}',");
                    
                }
                stringBuilder.Append(stringBuilderPart.ToString().TrimEnd(','));
                stringBuilderPart.Clear();
                stringBuilder.Append("},");
            }
            // (setting.ContractResolver as DefaultContractResolver).NamingStrategy = null;




            return new JsonResult($"[{stringBuilder.ToString().TrimEnd(',')}]");
        }
        public async Task<IActionResult> OnGetDataAsync([FromQuery] int stage = 1, int comofferid = 1)
        {
            var result = await _mediator.Send(new GetComOfferCrossStages { Stage = stage, ComOfferId = comofferid });
            if (!result.Succeeded)
            {
                return new JsonResult("");
            }
            return new JsonResult(result.Data);
            //var contragents = from s in result
            //                  from c in s.StageCompositions
            //                  group s by c.Contragent into contrs
            //                  select contrs.Key;

            //var ParticipantsDic = result.GroupBy(r => r.ComOffer).Select(g => g.Key)
            //                    .FirstOrDefault().ComParticipants.ToDictionary(k =>k.ContragentId,v=>v.Status);

            //var Participants = from c in result.GroupBy(r => r.ComOffer).Select(g => g.Key).FirstOrDefault().ComParticipants
            //                   select new
            //                   {
            //                       id = c.ContragentId,
            //                       status = c.Status
            //                   };
            //var status = from s in contragents
            //             join p in Participants on s.Id equals p.id into ps
            //             from p in ps.DefaultIfEmpty()
            //             select new ForTableHeader
            //             {
            //                 Contragent = s,
            //                 ParticipantStatus = p == null ? "" : p.status.ToDescriptionString() 
            //             };
            //if (status?.Count()>0)
            //{
            //    //Create header json
                
               




            //    var header = CreateHeaderEasy(status);
            //    string resultSjong = "{" +
            //   $"\"stage\" : 0," +
            //   $"\"deadline\" : 0," +
            //   "\"header\":["+ header+"], \"dataRows\":[]}";
            //    return new JsonResult(new
            //    {
            //        headertbl = header,

            //    });
            //}
            
           // return new JsonResult("");
        }
        private string[] CreateHeaderEasy(IEnumerable<ForTableHeader> contragents)
        {
        
            return contragents.Select(c => $"{c.Contragent.Name}({c.ParticipantStatus}) ").ToArray();
        }
        private string CreateHeader(IEnumerable<ForTableHeader> contragents)
        {
            StringBuilder stringBuilderHeader = new StringBuilder();

            stringBuilderHeader.Append('{');
            stringBuilderHeader.Append($"\"field\":\"StageNumber\",");
            stringBuilderHeader.Append("\"width\":100,");
            stringBuilderHeader.Append("\"title\":\"Этап\"},");
            stringBuilderHeader.Append("{\"field\":\"NomName\",");
            stringBuilderHeader.Append("\"width\":180,");
            stringBuilderHeader.Append("\"sortable\":true,");

            stringBuilderHeader.Append("\"title\":\"Номерклатура\"},");

            StringBuilder stringBuilderPart = new StringBuilder();
            
            foreach (var contr in contragents)
            {
                var status = contr.ParticipantStatus;
                stringBuilderPart.AppendFormat("{0}\"field\":\"ContName_{1}\",", "{", contr.Contragent.Id);
                stringBuilderPart.Append("\"width\":120,");
                stringBuilderPart.AppendFormat("\"title\":\"{0} ({1})\"{2},", contr.Contragent.Name, status,"}");
                var priceColname = string.Format("ContName_{0}_price", contr.Contragent.Id);
                var priceColstatus = string.Format("ContName_{0}_status", contr.Contragent.Id);
                //stringBuilderPart.AppendFormat("\"formatter\": \"/Function((one,two)=>{3} return (two.{0}==0 ? ' ': two.{0}) +' '+ (two.{1} ? 'да': 'нет') ;{2})/\"{2},", priceColname, priceColstatus, "}", "{");

                stringBuilderPart.AppendFormat("{0}\"field\":\"ContName_{1}_price\",", "{", contr.Contragent.Id);
                stringBuilderPart.Append("\"width\":120,");
                stringBuilderPart.Append("\"title\":\"Цена\"},");
                stringBuilderPart.AppendFormat("{0}\"field\":\"ContName_{1}_status\",", "{", contr.Contragent.Id);
                stringBuilderPart.Append("\"width\":120,");
                stringBuilderPart.Append("\"title\":\"Статус\"},");

            }
            stringBuilderHeader.Append(stringBuilderPart.ToString().TrimEnd(','));

            stringBuilderPart.Clear();
            return  "\"header\": [" + stringBuilderHeader.ToString() + "]";
        }
        public async Task<IActionResult> OnGetDataAsyncOld([FromQuery] int stage=1,int comofferid=1)
        {
            // throw new Exception("Test log error 222 !!!!!!");

            var result = await _mediator.Send(new GetByStageQuery { Stage = stage, ComOfferId = comofferid });
            if (result is null)
            {
                return new JsonResult("");
            }
            var Nomenclatures = result.StageCompositions.GroupBy(c => new { c.ComPosition, c.ComStage } ).Select(c => new
            {
                ComPosition = c.Key.ComPosition,
                comStage=c.Key.ComStage,
                stageNumber=result.Number,
                participients = c.ToList(),

            });
            
            StringBuilder stringBuilderHeader = new StringBuilder();
            
                stringBuilderHeader.Append('{');
                stringBuilderHeader.Append($"\"field\":\"StageNumber\",");
            stringBuilderHeader.Append("\"width\":100,");
            stringBuilderHeader.Append("\"title\":\"Этап\"},");
                stringBuilderHeader.Append("{\"field\":\"NomName\",");
            stringBuilderHeader.Append("\"width\":180,");
            stringBuilderHeader.Append("\"sortable\":true,");
        
            stringBuilderHeader.Append("\"title\":\"Номерклатура\"},");
            
            StringBuilder stringBuilderPart = new StringBuilder();
            var nomenclature = Nomenclatures.FirstOrDefault();
                foreach (var part in nomenclature.participients)
                {
                var status = nomenclature.ComPosition.ComOffer.ComParticipants.FirstOrDefault(p => p.ContragentId == part.ContragentId && p.ComOfferId==comofferid)?.Status.ToDescriptionString();
                    stringBuilderPart.AppendFormat("{0}\"field\":\"ContName_{1}\",", "{", part.Contragent.Id);
                stringBuilderPart.Append("\"width\":100,");
                stringBuilderPart.AppendFormat("\"title\":\"{0} ({1})\",", part.Contragent.Name, status);
                var priceColname = string.Format("ContName_{0}_price", part.Contragent.Id);
                var priceColstatus = string.Format("ContName_{0}_status", part.Contragent.Id);
                stringBuilderPart.AppendFormat("\"formatter\": \"/Function((one,two)=>{3} return (two.{0}==0 ? ' ': two.{0}) +' '+ (two.{1} ? 'да': 'нет') ;{2})/\"{2},", priceColname, priceColstatus, "}","{");

                //stringBuilderPart.AppendFormat("{0}field:'ContName_{1}_price',","{", part.Contragent.Id);
                //stringBuilderPart.Append("title:'Цена'},");
                //stringBuilderPart.AppendFormat("{0}field:'ContName_{1}_status',","{", part.Contragent.Id);
                //stringBuilderPart.Append("title:'Статус'},");

            }
                stringBuilderHeader.Append(stringBuilderPart.ToString().TrimEnd(','));
            
            stringBuilderPart.Clear();
                

            string resultSjong = "{" +
                $"\"stage\" : {(stage==1 ? "1": "")},"+
                $"\"deadline\" : {result.Deadline}," +
                "\"header\": [" + stringBuilderHeader.ToString()+"]," ;
            stringBuilderHeader.Clear();
            //var result = await _mediator.Send(command);
            foreach (var nomenclatured in Nomenclatures)
            {
                stringBuilderHeader.Append('{');
                stringBuilderHeader.Append($"\"StageNumber\":{nomenclatured.stageNumber},");
                
                stringBuilderHeader.Append($"\"NomName\":\"{nomenclatured.ComPosition.Nomenclature.Name}\",");
               // StringBuilder stringBuilderPart = new StringBuilder();
                foreach (var part in nomenclatured.participients)
                {
                    var status = nomenclatured.ComPosition.ComOffer.ComParticipants.FirstOrDefault(p => p.ContragentId == part.ContragentId)?.Status;
                    stringBuilderPart.AppendFormat("\"ContName_{0}\":{1},", part.Contragent.Id,part.ContragentId);
                    stringBuilderPart.AppendFormat("\"ContName_{0}_price\":{1},", part.Contragent.Id,part.Price);
                    stringBuilderPart.AppendFormat("\"ContName_{0}_status\":{1},", part.Contragent.Id,part.Status?"true":"false");
                }
                stringBuilderHeader.Append(stringBuilderPart.ToString().TrimEnd(','));
                stringBuilderPart.Clear();
                stringBuilderHeader.Append("},");
            }

            resultSjong+=" \"dataRows\": [" + stringBuilderHeader.ToString().TrimEnd(',') + "]}";


            
            
            return new JsonResult(resultSjong); ;

        

        
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                
                var result = await _mediator.Send(InputPos);
                
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
        public async Task<IActionResult> OnGetNomenclaturesAsync([FromQuery] int categoryid)
        {
            var command = new GetByCategoryIdNomenclatureQuery() { CatetoryId = categoryid };
            var result = await _mediator.Send(command);
            return new JsonResult(result);
        }
        public async Task<IActionResult> OnGetDeleteCheckedAsync([FromQuery] int[] id)
        {
            var command = new DeleteCheckedComStagesCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int id)
        {
            var command = new DeleteComStageCommand() { Id = id };
            var result = await _mediator.Send(command);
            return new JsonResult("");
        }
        public async Task<FileResult> OnPostExportAsync([FromBody] ExportComStagesQuery command)
        {
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["ComStages"] + ".xlsx");
        }
        public async Task<FileResult> OnGetCreateTemplate()
        {
            var command = new CreateComStagesTemplateCommand();
            var result = await _mediator.Send(command);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _localizer["ComStages"] + ".xlsx");
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
