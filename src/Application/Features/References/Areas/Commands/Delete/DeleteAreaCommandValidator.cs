using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Commands.Delete
{
    public class DeleteAreaCommandValidator : AbstractValidator<DeleteAreaCommand>
    {
        public DeleteAreaCommandValidator()
        {
           //TODO:Implementing DeleteAreaCommandValidator method 
            RuleFor(v => v.Id).NotNull().GreaterThan(0);
         
        }
    }
    public class DeleteCheckedAreasCommandValidator : AbstractValidator<DeleteCheckedAreasCommand>
    {
        public DeleteCheckedAreasCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
             RuleFor(v => v.Id).NotNull().NotEmpty();
            
        }
    }
}
