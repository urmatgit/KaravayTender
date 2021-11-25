using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.Create
{
    public class CreateComParticipantCommandValidator : AbstractValidator<CreateComParticipantCommand>
    {
        public CreateComParticipantCommandValidator()
        {
           //TODO:Implementing CreateComParticipantCommandValidator method 
            RuleFor(v => v.ContragentId)
            
                 .NotEmpty();
            RuleFor(v => v.ComOfferId)

                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
