using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.Create
{
    public class CreateContragentCategoryCommandValidator : AbstractValidator<CreateContragentCategoryCommand>
    {
        public CreateContragentCategoryCommandValidator()
        {
           //TODO:Implementing CreateContragentCategoryCommandValidator method 
            RuleFor(v => v.ContragentId)
                 
                 .NotEmpty();
            RuleFor(v => v.CategoryId)

                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
