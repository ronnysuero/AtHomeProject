using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace AtHomeProject.Domain.Models.Request
{
    [ExcludeFromCodeCoverage]
    [XmlRoot(ElementName = "Input")]
    public class Api3Request
    {
        [XmlElement(ElementName = nameof(Source))]
        public string Source { get; set; }

        [XmlElement(ElementName = nameof(Destination))]
        public string Destination { get; set; }

        [XmlArray(ElementName = nameof(Packages))]
        public IEnumerable<CartonDimensionModel> Packages { get; set; }
    }
}
