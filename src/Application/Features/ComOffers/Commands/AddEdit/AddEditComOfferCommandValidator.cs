using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.AddEdit
{
    public class AddEditComOfferCommandValidator : AbstractValidator<AddEditComOfferCommand>
    {
        public AddEditComOfferCommandValidator()
        {
           //TODO:Implementing AddEditComOfferCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(100)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
