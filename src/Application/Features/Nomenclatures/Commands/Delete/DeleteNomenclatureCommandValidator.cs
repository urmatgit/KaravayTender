using FluentValidation;

namespace CleanArchitecture.Razor.Application.Features.Nomenclatures.Commands.Delete
{
    public class DeleteNomenclatureCommandValidator : AbstractValidator<DeleteNomenclatureCommand>
    {
        public DeleteNomenclatureCommandValidator()
        {
           //TODO:Implementing DeleteNomenclatureCommandValidator method 
           //ex. RuleFor(v => v.Id).NotNull().GreaterThan(0);
           throw new System.NotImplementedException();
        }
    }
    public class DeleteCheckedNomenclaturesCommandValidator : AbstractValidator<DeleteCheckedNomenclaturesCommand>
    {
        public DeleteCheckedNomenclaturesCommandValidator()
        {
            //TODO:Implementing DeleteProductCommandValidator method 
            //ex. RuleFor(v => v.Id).NotNull().NotEmpty();
            throw new System.NotImplementedException();
        }
    }
}
