using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Commands.Update
{
    public class UpdateUnitOfCommandValidator : AbstractValidator<UpdateUnitOfCommand>
    {
        public UpdateUnitOfCommandValidator()
        {
           //TODO:Implementing UpdateUnitOfCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
