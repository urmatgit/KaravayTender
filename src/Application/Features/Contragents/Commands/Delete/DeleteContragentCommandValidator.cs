using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.Delete
{
    public class DeleteContragentCommandValidator : AbstractValidator<DeleteContragentCommand>
    {
        public DeleteContragentCommandValidator()
        {
           //TODO:Implementing DeleteContragentCommandValidator method 
           //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
           throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedContragentsCommandValidator : AbstractValidator<DeleteCheckedContragentsCommand>
    {
        public DeleteCheckedContragentsCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().NotEmpty();
            throw new System.NotImplementedException();
        }
    }
}
