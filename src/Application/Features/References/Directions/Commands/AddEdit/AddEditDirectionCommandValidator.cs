using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Directions.Commands.AddEdit
{
    public class AddEditDirectionCommandValidator : AbstractValidator<AddEditDirectionCommand>
    {
        public AddEditDirectionCommandValidator()
        {
           //TODO:Implementing AddEditDirectionCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
