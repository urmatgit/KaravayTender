using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.References.QualityDocs.Commands.Delete
{
    public class DeleteQualityDocCommandValidator : AbstractValidator<DeleteQualityDocCommand>
    {
        public DeleteQualityDocCommandValidator()
        {
           //TODO:Implementing DeleteQualityDocCommandValidator method 
           //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
           throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedQualityDocsCommandValidator : AbstractValidator<DeleteCheckedQualityDocsCommand>
    {
        public DeleteCheckedQualityDocsCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().NotEmpty();
            throw new System.NotImplementedException();
        }
    }
}
