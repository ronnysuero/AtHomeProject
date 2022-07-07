using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace AtHomeProject.Domain.Models.Response
{
    [ExcludeFromCodeCoverage]
    [XmlRoot(ElementName = "Response")]
    public record Api3Response(double Quote)
    {
        [XmlElement(ElementName = nameof(Quote))]
        public double Quote { get; set; } = Quote;
    }
}
