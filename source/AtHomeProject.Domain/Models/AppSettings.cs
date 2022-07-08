using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public record AppSettings
    {
        public string Secret { get; set; }
    }
}
