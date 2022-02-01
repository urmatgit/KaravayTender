// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.Delete
{
    public class DeleteContragentCommandValidator : AbstractValidator<DeleteContragentCommand>
    {
        public DeleteContragentCommandValidator()
        {
            //TODO:Implementing DeleteContragentCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
            RuleFor(v => v.Id).NotNull().GreaterThan(0);
        }
    }
    public class DeleteCheckedContragentsCommandValidator : AbstractValidator<DeleteCheckedContragentsCommand>
    {
        public DeleteCheckedContragentsCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().NotEmpty();
            RuleFor(v => v.Id).NotNull().NotEmpty();

        }
    }
}
