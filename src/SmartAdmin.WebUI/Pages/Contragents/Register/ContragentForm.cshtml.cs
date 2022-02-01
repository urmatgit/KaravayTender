// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using CleanArchitecture.Razor.Application.Features.Categories.DTOs;
using CleanArchitecture.Razor.Application.Features.Contragents.Commands.AddEdit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartAdmin.WebUI.Pages.Shared.Components.Contragent
{
    public class ContragentForm
    {
        [BindProperty]
        public AddEditContragentCommand Input { get; set; }
        public SelectList Directions { get; set; }
        public List<CategoryDto> Categories { get; set; } = new();
        //[BindProperty]
        //    public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
    }
}
