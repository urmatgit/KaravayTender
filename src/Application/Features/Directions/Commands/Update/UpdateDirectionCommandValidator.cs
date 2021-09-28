using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Directions.Commands.Update
{
    public class UpdateDirectionCommandValidator : AbstractValidator<UpdateDirectionCommand>
    {
        public UpdateDirectionCommandValidator()
        {
           //TODO:Implementing UpdateDirectionCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
