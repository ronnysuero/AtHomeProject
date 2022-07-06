using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AtHomeProject.Web.Auth
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDeviceService _deviceService;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, IDeviceService deviceService)
        {
            _next = next;
            _deviceService = deviceService;
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
                var value = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(DeviceModel.SerialNumber))?.Value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    // attach device to context on successful jwt validation
                    context.Items["User"] = await _deviceService.GetBySerialNumberAsync(value);
                    return;
                }

                // attach default user to context on successful jwt validation
                context.Items["User"] = _appSettings.DefaultCredentials;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
