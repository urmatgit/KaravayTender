using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.AddEdit
{
    public class AddEditContragentCommandValidator : AbstractValidator<AddEditContragentCommand>
    {
        public AddEditContragentCommandValidator()
        {
           //TODO:Implementing AddEditContragentCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();

           //throw new System.NotImplementedException();
        }
    }
}
