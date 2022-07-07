using AtHomeProject.Domain.Models;
using FluentValidation;

namespace AtHomeProject.Domain.Validators
{
    public class CartonDimensionModelValidator : AbstractValidator<CartonDimensionModel>
    {
        public CartonDimensionModelValidator()
        {
            RuleFor(x => x.Package).NotEmpty().NotNull().SetValidator(new PackageDimensionModelValidator());
        }
    }
}
