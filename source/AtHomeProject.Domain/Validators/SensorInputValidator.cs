using System.Linq;
using AtHomeProject.Domain.Models;
using FluentValidation;

namespace AtHomeProject.Domain.Validators
{
    public class SensorInputValidator : AbstractValidator<SensorInputModel>
    {
        private readonly string[] _validHealthStatus = { "ok", "needs_filter", "needs_service" };

        public SensorInputValidator()
        {
            RuleFor(x => x.HealthStatus).NotEmpty().Must(value => _validHealthStatus.Contains(value?.ToLower()));
        }
    }
}
