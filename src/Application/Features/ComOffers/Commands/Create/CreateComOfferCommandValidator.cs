using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Create
{
    public class CreateComOfferCommandValidator : AbstractValidator<CreateComOfferCommand>
    {
        public CreateComOfferCommandValidator()
        {
           //TODO:Implementing CreateComOfferCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(100)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
