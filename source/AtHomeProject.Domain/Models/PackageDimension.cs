using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    [XmlRoot(ElementName = "package2")]
    public record PackageDimension
    {
        [XmlElement(ElementName = "width")]
        public int Width { get; set; }

        [XmlElement(ElementName = "height")]
        public int Height { get; set; }

        [XmlElement(ElementName = "weight")]
        public int Weight { get; set; }
    }
}
