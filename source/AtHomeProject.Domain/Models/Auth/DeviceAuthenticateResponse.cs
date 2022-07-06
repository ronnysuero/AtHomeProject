using System.Diagnostics.CodeAnalysis;

namespace AtHomeProject.Domain.Models.Auth
{
    [ExcludeFromCodeCoverage]
    public class DeviceAuthenticateResponse
    {
        public string SerialNumber { get; set; }

        public string Token { get; set; }

        public DeviceAuthenticateResponse(string serialNumber, string token)
        {
            SerialNumber = serialNumber;
            Token = token;
        }
    }
}
