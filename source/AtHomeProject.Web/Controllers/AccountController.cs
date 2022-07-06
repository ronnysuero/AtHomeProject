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
    [Route("Api/[controller]")]
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
        /// Login smartAC device
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "serialNumber": "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
        ///         "secretKey": "fff754c711b34ccd9bf1547f2ea96049",
        ///         "firmwareVersion": "1.0.1"
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>The jwt token and the serial number</returns>
        /// <response code="200">Returns the jwt token</response>
        /// <response code="400">If the model is null or an error occurred while was trying to authenticate</response>  
        [HttpPost("Auth/Device")]
        [ProducesResponseType(typeof(DeviceAuthenticateResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Authenticate(DeviceAuthenticateRequest model)
        {
            _logger.LogInformation(
                $"[${nameof(AccountController)}] loging called {DateTimeOffset.UtcNow}, device serial: {model.SerialNumber}"
            );

            var result = await _service.AuthenticateAsync(model);

            if (result is { })
            {
                _logger.LogInformation(
                    $"[${nameof(AccountController)}] loging called {DateTimeOffset.UtcNow}, device serial: {model.SerialNumber} sucessfully authenticated."
                );
                return Ok(result);
            }

            _logger.LogWarning(
                $"[${nameof(AccountController)}] loging called {DateTimeOffset.UtcNow}, device serial: {model.SerialNumber}, invalid credentials."
            );

            return BadRequest(
                new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Invalid credentials"
                }
            );
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "userName": "admin",
        ///         "password": "admin"
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>The jwt token and the serial number</returns>
        /// <response code="200">Returns the jwt token</response>
        /// <response code="400">If the model is null or an error occurred while was trying to authenticate</response>  
        [HttpPost("Auth/User")]
        [ProducesResponseType(typeof(DeviceAuthenticateResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public IActionResult Authenticate(UserModel model)
        {
            _logger.LogInformation(
                $"[${nameof(AccountController)}] loging called {DateTimeOffset.UtcNow}, user: {model.UserName}"
            );

            var result = _service.Authenticate(model);

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
