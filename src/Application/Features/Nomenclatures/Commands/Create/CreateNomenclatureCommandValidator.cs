using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.Create
{
    public class CreateNomenclatureCommandValidator : AbstractValidator<CreateNomenclatureCommand>
    {
        public CreateNomenclatureCommandValidator()
        {
           //TODO:Implementing CreateNomenclatureCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
