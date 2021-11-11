using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.Commands.Update
{
    public class UpdateQualityDocCommandValidator : AbstractValidator<UpdateQualityDocCommand>
    {
        public UpdateQualityDocCommandValidator()
        {
           //TODO:Implementing UpdateQualityDocCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
