// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.AddEdit
{
    public class AddEditContragentCommandValidator : AbstractValidator<AddEditContragentCommand>
    {
        public AddEditContragentCommandValidator()
        {
            //TODO:Implementing AddEditContragentCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
            RuleFor(v => v.FullName)
                 .MaximumLength(100)
                 .NotEmpty();
            RuleFor(v => v.INN)
                 .MaximumLength(12)
                 .NotEmpty();
            RuleFor(v => v.KPP)
                 .MaximumLength(20);
            RuleFor(v => v.Site)
                 .MaximumLength(50);

            RuleFor(v => v.Phone)
                 .MaximumLength(30)
                 .NotEmpty();
            RuleFor(v => v.ContactPerson)
                 .MaximumLength(30)
                 .NotEmpty();
            RuleFor(v => v.ContactPhone)
                 .MaximumLength(30)
                 .NotEmpty();
            RuleFor(v => v.Email)
                 .MaximumLength(30)
                 .NotEmpty();
            RuleFor(v => v.TypeOfActivity)
                 .MaximumLength(100)
                 .NotEmpty();
            RuleFor(v => v.DirectionId)
                .NotNull()
                .NotEqual(0)
                .WithMessage("Не выбран направление поставщика!");


            //throw new System.NotImplementedException();
        }
    }
}
