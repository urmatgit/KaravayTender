using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.Create
{
    public class CreateContragentCategoryCommandValidator : AbstractValidator<CreateContragentCategoryCommand>
    {
        public CreateContragentCategoryCommandValidator()
        {
           //TODO:Implementing CreateContragentCategoryCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
