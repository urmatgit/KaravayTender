// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SmartAdmin.WebUI.Extensions
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement(INPUTNAME)]
    public class KarInputTagHelper : TagHelper
    {
        private const string INPUTNAME = "asp-kar-input";
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }
        [HtmlAttributeName("label")]
        public string Label { get; set; }
        [HtmlAttributeName("holder")]
        public string holder { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.PreContent.SetHtmlContent($"<label class=\"form-label\" for=\"{For.Name}\"> {Label}<span class=\"text-danger\">*</span></label>");
            output.PostContent.SetHtmlContent($"<span class=\"invalid-feedback\" asp-validation-for=\"{For.Model}\"></span>");

            var tag = $"<input type=\"text\" name='{For.Name}' id='{For.Name.Replace('.', '_')}' class=\"form-control\" placeholder=\"{holder}\" />";
            output.TagName = "";
            //output.Attributes.Add("type", "text");
            //output.Attributes.Add("class", "form-control");
            //output.Attributes.Add("placeholder", "holder");
            //output.Attributes.Add("asp-for", $"{For}");

            output.Content.SetHtmlContent(tag);
        }
    }
}
