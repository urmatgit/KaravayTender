using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Categories.Commands.Delete
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
           //TODO:Implementing DeleteCategoryCommandValidator method 
           //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
           //throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedCategoriesCommandValidator : AbstractValidator<DeleteCheckedCategoriesCommand>
    {
        public DeleteCheckedCategoriesCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
