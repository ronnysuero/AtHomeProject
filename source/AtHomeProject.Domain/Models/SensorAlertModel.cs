using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using AtHomeProject.Data.Enum;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class SensorAlertModel
    {
        public int Id { get; set; }

        public string SerialNumber { get; set; }

        public DateTime DeviceRecorded { get; set; }

        [JsonIgnore] public static DateTime ServerReceived => DateTime.UtcNow;

        public DateTime? ResolvedAlert { get; set; }

        public string Description { get; set; }

        public double Value { get; set; }

        public ResolvedState ResolvedState { get; set; }

        public ViewState ViewState { get; set; }
    }
}
