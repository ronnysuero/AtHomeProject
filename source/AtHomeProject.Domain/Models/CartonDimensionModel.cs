using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class CartonDimensionModel
    {
        [XmlElement(ElementName = "Package")]
        public PackageDimensionModel Package { get; set; }
    }
}