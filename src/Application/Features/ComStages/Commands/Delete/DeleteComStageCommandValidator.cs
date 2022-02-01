using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Commands.Delete
{
    public class DeleteComStageCommandValidator : AbstractValidator<DeleteComStageCommand>
    {
        public DeleteComStageCommandValidator()
        {
           //TODO:Implementing DeleteComStageCommandValidator method 
           //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
           throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedComStagesCommandValidator : AbstractValidator<DeleteCheckedComStagesCommand>
    {
        public DeleteCheckedComStagesCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().NotEmpty();
            throw new System.NotImplementedException();
        }
    }
}
