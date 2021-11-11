using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Commands.Create
{
    public class CreateNomenclatureQualityDocCommandValidator : AbstractValidator<CreateNomenclatureQualityDocCommand>
    {
        public CreateNomenclatureQualityDocCommandValidator()
        {
           //TODO:Implementing CreateNomenclatureQualityDocCommandValidator method 
            RuleFor(v => v.NomenclatureId)
                 .NotEmpty();
            RuleFor(v => v.QualityDocId)
     .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
