using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.AddEdit
{
    public class AddEditStageParticipantCommandValidator : AbstractValidator<AddEditStageParticipantCommand>
    {
        public AddEditStageParticipantCommandValidator()
        {
            //TODO:Implementing AddEditStageParticipantCommandValidator method 
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
