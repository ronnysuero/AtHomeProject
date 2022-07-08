using AtHomeProject.Domain.Models.Request;
using FluentValidation;

namespace AtHomeProject.Domain.Validators
{
    public class Api3RequestValidator : AbstractValidator<Api3Request>
    {
        public Api3RequestValidator()
        {
            RuleFor(x => x.Source).NotEmpty().NotNull();
            RuleFor(x => x.Destination).NotEmpty().NotNull();
            RuleFor(x => x.Packages).NotEmpty().NotNull();
            RuleForEach(x => x.Packages).SetValidator(new PackageDimensionValidator());
        }
    }
}
