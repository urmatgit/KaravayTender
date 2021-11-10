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
