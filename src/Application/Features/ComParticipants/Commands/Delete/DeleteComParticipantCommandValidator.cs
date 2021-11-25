using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.ComParticipants.Commands.Delete
{
    public class DeleteComParticipantCommandValidator : AbstractValidator<DeleteComParticipantCommand>
    {
        public DeleteComParticipantCommandValidator()
        {
           //TODO:Implementing DeleteComParticipantCommandValidator method 
            RuleFor(v => v.ContragentId).NotNull().GreaterThan(0);
            RuleFor(v => v.ComOfferId).NotNull().GreaterThan(0);

        }
    }
     
}
