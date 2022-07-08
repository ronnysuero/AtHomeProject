using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Response
{
    [ExcludeFromCodeCoverage]
    public record Api2Response(double Amount)
    {
        public double Amount { get; set; } = Amount;
    }
}
