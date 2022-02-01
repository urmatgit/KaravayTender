using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.Update
{
    public class UpdateNomenclatureCommandValidator : AbstractValidator<UpdateNomenclatureCommand>
    {
        public UpdateNomenclatureCommandValidator()
        {
           //TODO:Implementing UpdateNomenclatureCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty();
           //throw new System.NotImplementedException();
        }
    }
}
