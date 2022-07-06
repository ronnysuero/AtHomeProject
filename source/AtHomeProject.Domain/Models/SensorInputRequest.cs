using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AtHomeProject.Domain.Models.Pagination;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class SensorInputRequest : PaginationRequest
    {
        [Required] public string SerialNumber { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}
