using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Request
{
    [ExcludeFromCodeCoverage]
    public record Api2Request
    {
        public string Consignee { get; set; }
        public string Consignor { get; set; }
        public IEnumerable<PackageDimension> Cartons { get; set; }
    }
}
