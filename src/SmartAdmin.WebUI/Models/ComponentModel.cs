// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SmartAdmin.WebUI.Models
{
    public class ComponentModel
    {
        public string Name { get; set; }
        public ModelExpression InputCustomer { get; set; }
        public string Label { get; set; }
        public string PlaceHolder { get; set; }
    }
}
