using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace AtHomeProject.Domain.Models.Request
{
    [ExcludeFromCodeCoverage]
    [XmlRoot(ElementName = nameof(Api3Request))]
    public record Api3Request
    {
        [XmlElement(ElementName = "source")]
        public string Source { get; set; }

        [XmlElement(ElementName = "destination")]
        public string Destination { get; set; }

        [XmlArray(ElementName = "packages")]
        [XmlArrayItem(ElementName = "package")]
        public PackageDimension[] Packages { get; set; }
    }
}
