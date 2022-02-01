// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Directions.Commands.Delete
{
    public class DeleteDirectionCommandValidator : AbstractValidator<DeleteDirectionCommand>
    {
        public DeleteDirectionCommandValidator()
        {
            //TODO:Implementing DeleteDirectionCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);

        }
    }
    public class DeleteCheckedDirectionsCommandValidator : AbstractValidator<DeleteCheckedDirectionsCommand>
    {
        public DeleteCheckedDirectionsCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().NotEmpty();

        }
    }
}
