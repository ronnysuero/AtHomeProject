using System;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class SensorInputListModel
    {
        public string SerialNumber { get; set; }

        public DateTime DeviceRecorded { get; set; }

        public string HealthStatus { get; set; }

        public double Humidity { get; set; }

        public double CarbonMonoxide { get; set; }

        public double Temperature { get; set; }
    }
}
