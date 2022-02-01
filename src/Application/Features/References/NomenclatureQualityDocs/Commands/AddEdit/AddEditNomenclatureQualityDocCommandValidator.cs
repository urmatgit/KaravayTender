using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Commands.AddEdit
{
    public class AddEditNomenclatureQualityDocCommandValidator : AbstractValidator<AddEditNomenclatureQualityDocCommand>
    {
        public AddEditNomenclatureQualityDocCommandValidator()
        {
           //TODO:Implementing AddEditNomenclatureQualityDocCommandValidator method 
            RuleFor(v => v.NomenclatureId)
                 .NotEmpty();
            RuleFor(v => v.QualityDoc)
            .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
