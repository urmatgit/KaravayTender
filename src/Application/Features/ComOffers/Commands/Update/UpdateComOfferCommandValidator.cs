using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Update
{
    public class UpdateComOfferCommandValidator : AbstractValidator<UpdateComOfferCommand>
    {
        public UpdateComOfferCommandValidator()
        {
           //TODO:Implementing UpdateComOfferCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(100)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
