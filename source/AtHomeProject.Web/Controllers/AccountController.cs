using System;
using System.Net;
using System.Threading.Tasks;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AtHomeProject.Web.Controllers
{
    [ApiController]
    [Route("v1/")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAuthService _service;

        public AccountController(ILogger<AccountController> logger, IAuthService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     API1
        ///     {
        ///         "userName": "user1",
        ///         "password": "admin1"
        ///     }
        ///
        ///     API2
        ///     {
        ///         "userName": "user2",
        ///         "password": "admin2"
        ///     }
        ///
        ///     XML
        ///     {
        ///         "userName": "user3",
        ///         "password": "admin3"
        ///     }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>The jwt token</returns>
        /// <response code="200">Returns the jwt token</response>
        /// <response code="400">If the model is null or an error occurred while was trying to authenticate</response>  
        [HttpPost("Auth")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Authenticate(UserModel model)
        {
            _logger.LogInformation(
                $"[${nameof(AccountController)}] loging called {DateTimeOffset.UtcNow}, user: {model.UserName}"
            );

            var result = await _service.AuthenticateAsync(model);

            if (result is { })
            {
                _logger.LogInformation(
                    $"[${nameof(AccountController)}] loging called {DateTimeOffset.UtcNow}, user: {model.UserName} sucessfully authenticated."
                );
                return Ok(result);
            }

            _logger.LogWarning(
                $"[${nameof(AccountController)}] loging called {DateTimeOffset.UtcNow}, user: {model.UserName}, invalid credentials."
            );

            return BadRequest(
                new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Invalid credentials"
                }
            );
        }
    }
}
