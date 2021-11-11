// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Commands.Create
{
    public class CreateUnitOfCommandValidator : AbstractValidator<CreateUnitOfCommand>
    {
        public CreateUnitOfCommandValidator()
        {
            //TODO:Implementing CreateUnitOfCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
