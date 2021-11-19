using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Commands.Update
{
    public class UpdateAreaCommandValidator : AbstractValidator<UpdateAreaCommand>
    {
        public UpdateAreaCommandValidator()
        {
           //TODO:Implementing UpdateAreaCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(100)
                 .NotEmpty();
            RuleFor(v => v.Address)
                 
                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
