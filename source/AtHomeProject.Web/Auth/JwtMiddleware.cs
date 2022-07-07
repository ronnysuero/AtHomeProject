using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AtHomeProject.Data.Interfaces;
using AtHomeProject.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static System.Int32;

namespace AtHomeProject.Web.Auth
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, IUnitOfWork unitOfWork)
        {
            _next = next;
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var hasAuthorizeAttribute = context.Features
                .Get<IEndpointFeature>()?
                .Endpoint?
                .Metadata.Any(m => m is AuthorizeAttribute);

            if (hasAuthorizeAttribute is true)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (!string.IsNullOrWhiteSpace(token))
                    await AttachUserToContext(context, token);
            }

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    },
                    out var validatedToken
                );

                var jwtToken = (JwtSecurityToken)validatedToken;
                var value = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(ClaimTypes.Actor))?.Value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    // attach user to context on successful jwt validation
                    TryParse(value, out var userId);
                    context.Items["User"] = await _unitOfWork.Users.FindAsync(f => f.Id == userId);
                    context.Items["UserClaims"] = await _unitOfWork.UsersClaims.GetAsync(f => f.UserId == userId);
                }
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
