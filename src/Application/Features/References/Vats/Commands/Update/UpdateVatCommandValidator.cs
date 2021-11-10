using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.Vats.Commands.Update
{
    public class UpdateVatCommandValidator : AbstractValidator<UpdateVatCommand>
    {
        public UpdateVatCommandValidator()
        {
           //TODO:Implementing UpdateVatCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
