using System;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class DeviceListModel
    {
        public string SerialNumber { get; set; }

        public DateTime? FirstRegistration { get; set; }

        public DateTime? LatestRegistration { get; set; }

        public string FirmwareVersion { get; set; }
    }
}
