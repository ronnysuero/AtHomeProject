using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Request
{
    [ExcludeFromCodeCoverage]
    public class Api1Request
    {
        public string ContactAddress { get; set; }
        public string WharehouseAddress { get; set; }
        public IEnumerable<PackageDimensionModel> Packages { get; set; }
    }
}
