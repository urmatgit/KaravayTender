using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComOffers.Commands.Delete
{
    public class DeleteComOfferCommandValidator : AbstractValidator<DeleteComOfferCommand>
    {
        public DeleteComOfferCommandValidator()
        {
           //TODO:Implementing DeleteComOfferCommandValidator method 
           RuleFor(v => v.Id).NotNull().GreaterThan(0);
           //throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedComOffersCommandValidator : AbstractValidator<DeleteCheckedComOffersCommand>
    {
        public DeleteCheckedComOffersCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
             RuleFor(v => v.Id).NotNull().NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
