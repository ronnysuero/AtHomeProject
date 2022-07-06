using AtHomeProject.Domain.Models;
using FluentValidation;

namespace AtHomeProject.Domain.Validators
{
    public class SensorInputRequestValidator : AbstractValidator<SensorInputRequest>
    {
        public SensorInputRequestValidator()
        {
            RuleFor(x => x.SerialNumber).NotEmpty();

            RuleFor(x => x.Page).NotEmpty().GreaterThan(0);

            RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
