using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.Areas.Commands.Create
{
    public class CreateAreaCommandValidator : AbstractValidator<CreateAreaCommand>
    {
        public CreateAreaCommandValidator()
        {
           //TODO:Implementing CreateAreaCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(100)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
