using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.AddEdit
{
    public class AddEditNomenclatureCommandValidator : AbstractValidator<AddEditNomenclatureCommand>
    {
        public AddEditNomenclatureCommandValidator()
        {
           //TODO:Implementing AddEditNomenclatureCommandValidator method 
            RuleFor(v => v.Name)
                 .MaximumLength(50)
                 .NotEmpty()
                 .WithMessage("'Наименование' является обязательным ");
            //throw new System.NotImplementedException();
        }
    }
}
