using System;
using System.Diagnostics.CodeAnalysis;
using AtHomeProject.Data.Enum;

namespace AtHomeProject.Data.Entities
{
    [ExcludeFromCodeCoverage]
    public class SensorAlert
    {
        public int Id { get; set; }

        public string SerialNumber { get; set; }

        public DateTime DeviceRecorded { get; set; }

        public DateTime ServerReceived { get; set; }

        public DateTime? ResolvedAlert { get; set; }

        public string Description { get; set; }

        public double Value { get; set; }

        public ResolvedState ResolvedState { get; set; }

        public ViewState ViewState { get; set; }
    }
}
