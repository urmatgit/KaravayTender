using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Contragents.Commands.Create
{
    public class CreateContragentCommandValidator : AbstractValidator<CreateContragentCommand>
    {
        public CreateContragentCommandValidator()
        {
           //TODO:Implementing CreateContragentCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
