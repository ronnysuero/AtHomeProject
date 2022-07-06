using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AtHomeProject.Data.Interfaces;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Semver;

namespace AtHomeProject.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public AuthService(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task<DeviceAuthenticateResponse> AuthenticateAsync(DeviceAuthenticateRequest model)
        {
            var device = await _unitOfWork.Device.FindAsync(f =>
                f.SerialNumber == model.SerialNumber &&
                f.SecretKey == model.SecretKey
            );

            // return null if device not found
            if (device == null)
                return null;

            device.FirstRegistration ??= DateTime.UtcNow;
            device.LatestRegistration = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(model.FirmwareVersion))
            {
                var firmwareVersion = SemVersion.Parse(model.FirmwareVersion, SemVersionStyles.Strict);

                if (device.FirmwareVersion < firmwareVersion)
                    device.FirmwareVersion = firmwareVersion;
            }

            await _unitOfWork.SaveAsync();

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(nameof(DeviceModel.SerialNumber), model.SerialNumber);

            return new DeviceAuthenticateResponse(model.SerialNumber, token);
        }

        public UserAuthenticateResponse Authenticate(UserModel model)
        {
            var validUser =
                _appSettings.DefaultCredentials is { UserName: { }, Password: { } } &&
                _appSettings.DefaultCredentials.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase) &&
                _appSettings.DefaultCredentials.Password.Equals(model.Password);

            // return null if credentials are not valid
            if (!validUser)
                return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(nameof(UserModel.UserName), model.UserName);

            return new UserAuthenticateResponse(model.UserName, token);
        }

        private string GenerateJwtToken(string claimType, string value)
        {
            // Token will expire in 30 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(claimType, value) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
