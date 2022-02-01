// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Commands.Delete
{
    public class DeleteVatCommandValidator : AbstractValidator<DeleteVatCommand>
    {
        public DeleteVatCommandValidator()
        {
            //TODO:Implementing DeleteVatCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
            throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedVatsCommandValidator : AbstractValidator<DeleteCheckedVatsCommand>
    {
        public DeleteCheckedVatsCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            RuleFor(v => v.Id).NotNull().NotEmpty();

        }
    }
}
