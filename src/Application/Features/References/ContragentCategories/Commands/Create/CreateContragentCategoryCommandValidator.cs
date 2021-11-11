// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.Create
{
    public class CreateContragentCategoryCommandValidator : AbstractValidator<CreateContragentCategoryCommand>
    {
        public CreateContragentCategoryCommandValidator()
        {
            //TODO:Implementing CreateContragentCategoryCommandValidator method 
            RuleFor(v => v.ContragentId)

                 .NotEmpty();
            RuleFor(v => v.CategoryId)

                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
