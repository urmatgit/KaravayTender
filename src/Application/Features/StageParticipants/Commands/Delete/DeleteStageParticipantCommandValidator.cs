using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.StageParticipants.Commands.Delete
{
    public class DeleteStageParticipantCommandValidator : AbstractValidator<DeleteStageParticipantCommand>
    {
        public DeleteStageParticipantCommandValidator()
        {
           //TODO:Implementing DeleteStageParticipantCommandValidator method 
           //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
           throw new System.NotImplementedException();
        }
    }
    //public class DeleteCheckedStageParticipantsCommandValidator : AbstractValidator<DeleteCheckedStageParticipantsCommand>
    //{
    //    public DeleteCheckedStageParticipantsCommandValidator()
    //    {
    //        //TODO:Implementing DeleteProductCommandValidator method 
    //        //ex. RuleFor(v => v.Id).NotNull().NotEmpty();
    //        throw new System.NotImplementedException();
    //    }
    //}
}
