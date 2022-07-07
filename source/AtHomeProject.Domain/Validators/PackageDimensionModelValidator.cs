using AtHomeProject.Domain.Models;
using FluentValidation;

namespace AtHomeProject.Domain.Validators
{
    public class PackageDimensionModelValidator : AbstractValidator<PackageDimensionModel>
    {
        public PackageDimensionModelValidator()
        {
            RuleFor(x => x.Height).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Weight).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Width).NotEmpty().GreaterThan(0);
        }
    }
}
