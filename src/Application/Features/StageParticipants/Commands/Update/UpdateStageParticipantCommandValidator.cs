using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.Update
{
    public class UpdateStageParticipantCommandValidator : AbstractValidator<UpdateStageParticipantCommand>
    {
        public UpdateStageParticipantCommandValidator()
        {
           //TODO:Implementing UpdateStageParticipantCommandValidator method 
            RuleFor(v => v.ComOfferId)
                 .NotEmpty();
            RuleFor(v => v.ComStageId)
                 .NotEmpty();
            RuleFor(v => v.ContragentId)
                 .NotEmpty();



            //throw new System.NotImplementedException();
        }
    }
}
