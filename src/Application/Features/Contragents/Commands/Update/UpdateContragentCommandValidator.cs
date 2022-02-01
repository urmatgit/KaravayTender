// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.Update
{
    public class UpdateContragentCommandValidator : AbstractValidator<UpdateContragentCommand>
    {
        public UpdateContragentCommandValidator()
        {
            //TODO:Implementing UpdateContragentCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
