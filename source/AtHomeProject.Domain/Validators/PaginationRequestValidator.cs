using AtHomeProject.Domain.Models.Pagination;
using FluentValidation;

namespace AtHomeProject.Domain.Validators
{
    public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
    {
        public PaginationRequestValidator()
        {
            RuleFor(x => x.Page).NotEmpty().GreaterThan(0);

            RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
