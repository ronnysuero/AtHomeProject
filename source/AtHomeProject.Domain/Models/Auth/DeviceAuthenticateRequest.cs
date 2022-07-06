using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Auth
{
    [ExcludeFromCodeCoverage]
    public class DeviceAuthenticateRequest
    {
        [Required] public string SerialNumber { get; set; }

        [Required] public string SecretKey { get; set; }

        public string FirmwareVersion { get; set; }
    }
}
