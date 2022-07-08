using AtHomeProject.Domain.Models.Request;
using FluentValidation;

namespace AtHomeProject.Domain.Validators
{
    public class Api2RequestValidator : AbstractValidator<Api2Request>
    {
        public Api2RequestValidator()
        {
            RuleFor(x => x.Consignee).NotEmpty().NotNull();
            RuleFor(x => x.Consignor).NotEmpty().NotNull();
            RuleFor(x => x.Cartons).NotEmpty().NotNull();
            RuleForEach(x => x.Cartons).SetValidator(new PackageDimensionValidator());
        }
    }
}
