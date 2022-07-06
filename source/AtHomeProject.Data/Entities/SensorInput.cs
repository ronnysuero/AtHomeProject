using System;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Data.Entities
{
    [ExcludeFromCodeCoverage]
    public class SensorInput
    {
        public int Id { get; set; }

        public string SerialNumber { get; set; }

        public DateTime DeviceRecorded { get; set; }

        public DateTime ServerReceived { get; set; }

        public string HealthStatus { get; set; }

        public double Humidity { get; set; }

        public double CarbonMonoxide { get; set; }

        public double Temperature { get; set; }
    }
}
