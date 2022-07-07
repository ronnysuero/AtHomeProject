using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Request
{
    [ExcludeFromCodeCoverage]
    public class Api2Request
    {
        public string Consignee { get; set; }
        public string Consignor { get; set; }
        public IEnumerable<PackageDimensionModel> Cartons { get; set; }
    }
}
