using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.UnitOfs.Commands.AddEdit
{
    public class AddEditUnitOfCommandValidator : AbstractValidator<AddEditUnitOfCommand>
    {
        public AddEditUnitOfCommandValidator()
        {
           //TODO:Implementing AddEditUnitOfCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
