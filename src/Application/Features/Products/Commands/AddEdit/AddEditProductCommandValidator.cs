// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Products.Commands.AddEdit
{
    public class AddEditProductCommandValidator : AbstractValidator<AddEditProductCommand>
    {
        public AddEditProductCommandValidator()
        {
            //TODO:Implementing AddEditProductCommandValidator method 
            RuleFor(v => v.Name)
                  .MaximumLength(256)
                  .NotEmpty();
        }
    }
}
