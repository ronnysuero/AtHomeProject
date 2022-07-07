using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AtHomeProject.Data.Entities;
using AtHomeProject.Data.Interfaces;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<AuthResponse> AuthenticateAsync(UserModel model)
        {
            var user = await _unitOfWork.Users.FindAsync(f =>
                    f.Username == model.UserName &&
                    f.Password == model.Password //TODO: password should be hashed
            );

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);

            return new AuthResponse(token);
        }

        private string GenerateJwtToken(Users user)
        {
            var claims = user.Claims.Select(s => new Claim(s.ClaimType, s.ClaimType)).ToList();
            claims.Add(new Claim(nameof(ClaimTypes.Actor), user.Id.ToString()));

            // Token will expire in 30 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
