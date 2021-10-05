using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ContragentCategories.Commands.Update
{
    public class UpdateContragentCategoryCommandValidator : AbstractValidator<UpdateContragentCategoryCommand>
    {
        public UpdateContragentCategoryCommandValidator()
        {
           //TODO:Implementing UpdateContragentCategoryCommandValidator method 
            RuleFor(v => v.ContragentId)
                 .NotEmpty();
            RuleFor(v => v.CategoryId)
            .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
