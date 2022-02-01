// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Commands.Create
{
    public class CreateVatCommandValidator : AbstractValidator<CreateVatCommand>
    {
        public CreateVatCommandValidator()
        {
            //TODO:Implementing CreateVatCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
