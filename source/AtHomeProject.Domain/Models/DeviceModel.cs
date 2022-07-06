using System;
using System.Diagnostics.CodeAnalysis;
using Semver;

namespace AtHomeProject.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class DeviceModel
    {
        public string SerialNumber { get; set; }

        public string SecretKey { get; set; }

        public DateTime? FirstRegistration { get; set; }

        public DateTime? LatestRegistration { get; set; }

        public SemVersion FirmwareVersion { get; set; }
    }
}
