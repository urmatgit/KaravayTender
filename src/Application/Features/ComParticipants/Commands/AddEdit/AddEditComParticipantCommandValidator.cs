using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.AddEdit
{
    public class AddEditComParticipantCommandValidator : AbstractValidator<AddEditComParticipantCommand>
    {
        public AddEditComParticipantCommandValidator()
        {
           //TODO:Implementing AddEditComParticipantCommandValidator method 
            RuleFor(v => v.ContragentId)
                 
                 .NotEmpty();
            RuleFor(v => v.ComOfferId)

                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
