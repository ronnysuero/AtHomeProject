using AtHomeProject.Domain.Models.Request;
using FluentValidation;

namespace AtHomeProject.Domain.Validators
{
    public class Api1RequestValidator : AbstractValidator<Api1Request>
    {
        public Api1RequestValidator()
        {
            RuleFor(x => x.ContactAddress).NotEmpty().NotNull();
            RuleFor(x => x.WharehouseAddress).NotEmpty().NotNull();
            RuleFor(x => x.Packages).NotEmpty().NotNull();
            RuleForEach(x => x.Packages).SetValidator(new PackageDimensionValidator());
        }
    }
}
