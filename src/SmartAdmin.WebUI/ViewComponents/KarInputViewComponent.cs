// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SmartAdmin.WebUI.Models;

namespace SmartAdmin.WebUI.ViewComponents
{
    public class KarInputViewComponent: ViewComponent
    {
        public KarInputViewComponent()
        {

        }
        public IViewComponentResult Invoke(string name,string label, string placeholder, ModelExpression aspModel)
        {
            var componentModel = new ComponentModel()
            {
                Name = name,
                InputCustomer = aspModel,
                Label = label,
                PlaceHolder=placeholder
            };
            return View(componentModel);
        }
    }
}
