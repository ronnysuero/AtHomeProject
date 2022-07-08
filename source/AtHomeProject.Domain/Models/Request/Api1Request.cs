using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Request
{
    [ExcludeFromCodeCoverage]
    public record Api1Request
    {
        public string ContactAddress { get; set; }
        public string WharehouseAddress { get; set; }
        public IEnumerable<PackageDimension> Packages { get; set; }
    }
}
