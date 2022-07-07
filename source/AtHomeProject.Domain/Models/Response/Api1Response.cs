using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Response
{
    [ExcludeFromCodeCoverage]
    public record Api1Response(double Total)
    {
        public double Total { get; set; } = Total;
    }
}
