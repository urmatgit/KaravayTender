using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComPositions.Commands.Delete
{
    public class DeleteComPositionCommandValidator : AbstractValidator<DeleteComPositionCommand>
    {
        public DeleteComPositionCommandValidator()
        {
           //TODO:Implementing DeleteComPositionCommandValidator method 
            RuleFor(v => v.Id).NotNull().GreaterThan(0);
           //throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedComPositionsCommandValidator : AbstractValidator<DeleteCheckedComPositionsCommand>
    {
        public DeleteCheckedComPositionsCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
             RuleFor(v => v.Id).NotNull().NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
