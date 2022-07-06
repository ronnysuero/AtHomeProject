using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace AtHomeProject.Domain.Models.Pagination
{
    [ExcludeFromCodeCoverage]
    [XmlRoot(ElementName = nameof(PaginationRequest))]
    public class PaginationRequest
    {
        [Required] 
        [XmlElement(ElementName = nameof(Page))]
        public int Page { get; set; }

        [Required] 
        [XmlElement(ElementName = nameof(PageSize))]
        public int PageSize { get; set; }
    }
}
