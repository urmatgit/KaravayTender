using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.Create
{
    public class CreateStageParticipantCommandValidator : AbstractValidator<CreateStageParticipantCommand>
    {
        public CreateStageParticipantCommandValidator()
        {
           //TODO:Implementing CreateStageParticipantCommandValidator method 
            RuleFor(v => v.ComOfferId)
                 .NotEmpty();
            RuleFor(v => v.ContragentId)
     .NotEmpty();
            RuleFor(v => v.ComStageId)
     .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
