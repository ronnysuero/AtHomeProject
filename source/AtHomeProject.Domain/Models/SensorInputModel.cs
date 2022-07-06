using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class SensorInputModel
    {
        [Required] public string SerialNumber { get; set; }

        public DateTime DeviceRecorded { get; set; }

        [JsonIgnore] public static DateTime ServerReceived => DateTime.UtcNow;

        [Required] public string HealthStatus { get; set; }

        public double Humidity { get; set; }

        public double CarbonMonoxide { get; set; }

        public double Temperature { get; set; }
    }
}
