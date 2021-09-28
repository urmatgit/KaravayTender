using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.AddEdit
{
    public class AddEditContragentCategoryCommandValidator : AbstractValidator<AddEditContragentCategoryCommand>
    {
        public AddEditContragentCategoryCommandValidator()
        {
           //TODO:Implementing AddEditContragentCategoryCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
