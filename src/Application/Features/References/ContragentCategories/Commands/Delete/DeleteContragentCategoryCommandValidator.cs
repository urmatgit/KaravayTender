// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.Delete
{
    public class DeleteContragentCategoryCommandValidator : AbstractValidator<DeleteContragentCategoryCommand>
    {
        public DeleteContragentCategoryCommandValidator()
        {
            //TODO:Implementing DeleteContragentCategoryCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
            throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedContragentCategoriesCommandValidator : AbstractValidator<DeleteCheckedContragentCategoriesCommand>
    {
        public DeleteCheckedContragentCategoriesCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().NotEmpty();
            throw new System.NotImplementedException();
        }
    }
}
