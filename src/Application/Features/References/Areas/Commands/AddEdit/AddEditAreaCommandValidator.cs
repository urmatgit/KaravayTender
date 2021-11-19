using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Commands.AddEdit
{
    public class AddEditAreaCommandValidator : AbstractValidator<AddEditAreaCommand>
    {
        public AddEditAreaCommandValidator()
        {
           //TODO:Implementing AddEditAreaCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(100)
                 .NotEmpty();
            RuleFor(v => v.Address)
                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
