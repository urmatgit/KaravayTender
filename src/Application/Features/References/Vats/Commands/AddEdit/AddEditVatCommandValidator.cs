using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Commands.AddEdit
{
    public class AddEditVatCommandValidator : AbstractValidator<AddEditVatCommand>
    {
        public AddEditVatCommandValidator()
        {
           //TODO:Implementing AddEditVatCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
