// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.Update
{
    public class UpdateContragentCategoryCommandValidator : AbstractValidator<UpdateContragentCategoryCommand>
    {
        public UpdateContragentCategoryCommandValidator()
        {
            //TODO:Implementing UpdateContragentCategoryCommandValidator method 
            RuleFor(v => v.ContragentId)
                 .NotEmpty();
            RuleFor(v => v.CategoryId)
            .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
