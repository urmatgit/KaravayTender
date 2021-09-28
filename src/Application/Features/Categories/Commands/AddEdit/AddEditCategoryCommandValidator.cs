using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.AddEdit
{
    public class AddEditCategoryCommandValidator : AbstractValidator<AddEditCategoryCommand>
    {
        public AddEditCategoryCommandValidator()
        {
           //TODO:Implementing AddEditCategoryCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
