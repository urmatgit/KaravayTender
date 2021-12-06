using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComStages.Commands.Create
{
    public class CreateComStageCommandValidator : AbstractValidator<CreateComStageCommand>
    {
        public CreateComStageCommandValidator()
        {
           //TODO:Implementing CreateComStageCommandValidator method 
            RuleFor(v => v.Number)
                 .NotEmpty().NotEqual(0);
            RuleFor(v=>v.Deadline)
                 .NotEmpty().NotEqual(0);
            //throw new System.NotImplementedException();
        }
    }
}
