using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Commands.Create
{
    public class CreateUnitOfCommandValidator : AbstractValidator<CreateUnitOfCommand>
    {
        public CreateUnitOfCommandValidator()
        {
           //TODO:Implementing CreateUnitOfCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
