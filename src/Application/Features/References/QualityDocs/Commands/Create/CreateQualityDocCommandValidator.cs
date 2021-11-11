using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.Commands.Create
{
    public class CreateQualityDocCommandValidator : AbstractValidator<CreateQualityDocCommand>
    {
        public CreateQualityDocCommandValidator()
        {
           //TODO:Implementing CreateQualityDocCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
