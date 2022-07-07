using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Response
{
    [ExcludeFromCodeCoverage]
    public record Api2Response(double Total)
    {
        public double Total { get; set; } = Total;
    }
}
