// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Application.Features.Categories.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.Directions.DTOs;
using CleanArchitecture.Razor.Application.Features.Directions.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.References.Areas.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.References.QualityDocs.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.References.UnitOfs.Queries.GetAll;
using CleanArchitecture.Razor.Application.Features.References.Vats.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartAdmin.WebUI.Extensions
{
    public static class References
    {
        public  static async Task<SelectList> LoadDirection(this ISender _mediator,bool hideService=false)
        {
            var request = new GetAllDirectionsQuery() {HideService=hideService };
            var directionsDtos = (List<DirectionDto>)await _mediator.Send(request);
            //Debug.WriteLine(JsonConvert.SerializeObject(directionsDtos));
            //Input.Directions = directionsDtos;
            return new SelectList(directionsDtos, "Id", "Name");
        }
        public static async Task<SelectList> LoadCategory(this ISender _mediator,int directionid)
        {
            var command = new GetAllCategoriesQuery() { DirectionId = directionid };
            var result = await _mediator.Send(command);
            return new SelectList(result, "Id", "Name", null, "DirectionName") ;   
        }
        public static async Task<SelectList> LoadUnitOf(this ISender _mediator)
        {
            var command = new GetAllUnitOfsQuery();
            var result = await _mediator.Send(command);
            return new SelectList(result, "Id", "Name");
        }
        public static async Task<SelectList> LoadVats(this ISender _mediator)
        {
            var command = new GetAllVatsQuery();
            var result = await _mediator.Send(command);
            return new SelectList(result, "Id", "Name");
        }
        public static async Task<SelectList> LoadQualityDocs(this ISender _mediator)
        {
            var command = new GetAllQualityDocsQuery();
            var result = await _mediator.Send(command);
            return new SelectList(result, "Id", "Name");
        }
        public static async Task<SelectList> LoadAreas(this ISender _mediator)
        {
            var command = new GetAllAreasQuery();
            var result = await _mediator.Send(command);
            return new SelectList(result, "Id", "Name");
        }
    }
}
