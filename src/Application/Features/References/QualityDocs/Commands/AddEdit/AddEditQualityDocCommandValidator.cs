using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.Commands.AddEdit
{
    public class AddEditQualityDocCommandValidator : AbstractValidator<AddEditQualityDocCommand>
    {
        public AddEditQualityDocCommandValidator()
        {
           //TODO:Implementing AddEditQualityDocCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
