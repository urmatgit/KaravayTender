// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Commands.Delete
{
    public class DeleteUnitOfCommandValidator : AbstractValidator<DeleteUnitOfCommand>
    {
        public DeleteUnitOfCommandValidator()
        {
            //TODO:Implementing DeleteUnitOfCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
            throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedUnitOfsCommandValidator : AbstractValidator<DeleteCheckedUnitOfsCommand>
    {
        public DeleteCheckedUnitOfsCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            RuleFor(v => v.Id).NotNull().NotEmpty();

        }
    }
}