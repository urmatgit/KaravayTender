using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.Update
{
    public class UpdateComParticipantCommandValidator : AbstractValidator<UpdateComParticipantCommand>
    {
        public UpdateComParticipantCommandValidator()
        {
           //TODO:Implementing UpdateComParticipantCommandValidator method 
            RuleFor(v => v.ContragentId)
            
                 .NotEmpty();
            RuleFor(v => v.ComOfferId)

                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
