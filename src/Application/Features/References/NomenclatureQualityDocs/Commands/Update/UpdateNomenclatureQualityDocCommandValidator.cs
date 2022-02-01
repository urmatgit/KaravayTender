using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.NomenclatureQualityDocs.Commands.Update
{
    public class UpdateNomenclatureQualityDocCommandValidator : AbstractValidator<UpdateNomenclatureQualityDocCommand>
    {
        public UpdateNomenclatureQualityDocCommandValidator()
        {
           //TODO:Implementing UpdateNomenclatureQualityDocCommandValidator method 
            RuleFor(v => v.QualityDocId)
                 
                 .NotEmpty();
            RuleFor(v => v.NomenclatureId)

                 .NotEmpty();
            //throw new System.NotImplementedException();
        }
    }
}
