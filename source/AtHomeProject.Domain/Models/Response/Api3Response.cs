using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace AtHomeProject.Domain.Models.Response
{
    [ExcludeFromCodeCoverage]
    [XmlRoot(ElementName = "response")]
    public record Api3Response
    {
        public Api3Response()
        {
        }

        public Api3Response(double quote) => Quote = quote;

        [XmlElement(ElementName = "quote")]
        public double Quote { get; set; }
    }
}
