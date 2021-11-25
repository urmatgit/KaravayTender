using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.AreaComPositions.Commands.Delete
{
    public class DeleteAreaComPositionCommandValidator : AbstractValidator<DeleteAreaComPositionCommand>
    {
        public DeleteAreaComPositionCommandValidator()
        {
           //TODO:Implementing DeleteAreaComPositionCommandValidator method 
           //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
           throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedAreaComPositionsCommandValidator : AbstractValidator<DeleteCheckedAreaComPositionsCommand>
    {
        public DeleteCheckedAreaComPositionsCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().NotEmpty();
            throw new System.NotImplementedException();
        }
    }
}
