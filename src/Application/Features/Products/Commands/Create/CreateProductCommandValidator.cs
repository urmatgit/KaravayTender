// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Products.Commands.Create
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            //TODO:Implementing CreateProductCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(256)
                 .NotEmpty();
        }
    }
}
