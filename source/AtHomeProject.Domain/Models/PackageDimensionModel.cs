using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class PackageDimensionModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
    }
}
