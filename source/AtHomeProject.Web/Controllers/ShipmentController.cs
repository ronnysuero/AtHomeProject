using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models.Request;
using AtHomeProject.Domain.Models.Response;
using AtHomeProject.Web.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AtHomeProject.Web.Controllers
{
    [ApiController]
    [Route("v1/")]
    public class ShipmentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompanyService _companyService;

        public ShipmentController(ILogger<ShipmentController> logger, ICompanyService companyService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
        }

        /// <summary>
        /// Get total shipping cost based on companies API.
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Amount in USD</response>
        /// <response code="400">Bad request</response> 
        /// <response code="401">If user is not authorized</response>
        [Authorize(Constants.API_JSON1)]
        [HttpPost("[action]")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Api1Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Api1(Api1Request request)
        {
            _logger.LogInformation(
                $"[${nameof(ShipmentController)}] loging called {DateTimeOffset.UtcNow}, method {nameof(Api1)}"
            );

            var result = await _companyService.CalculateShipmentAsync(
                request.ContactAddress,
                request.WharehouseAddress,
                request.Packages
            );

            _logger.LogInformation(
                $"[${nameof(ShipmentController)}] loging called {DateTimeOffset.UtcNow}, method {nameof(Api1)}, result: {result}"
            );

            return Ok(new Api1Response(result));
        }

        /// <summary>
        /// Get total shipping cost based on companies API.
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Amount in USD</response>
        /// <response code="400">Bad request</response> 
        /// <response code="401">If user is not authorized</response>
        [Authorize(Constants.API_JSON2)]
        [HttpPost("[action]")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Api2Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Api2(Api2Request request)
        {
            _logger.LogInformation(
                $"[${nameof(ShipmentController)}] loging called {DateTimeOffset.UtcNow}, method {nameof(Api2)}"
            );

            var result = await _companyService.CalculateShipmentAsync(
                request.Consignee,
                request.Consignor,
                request.Cartons
            );

            _logger.LogInformation(
                $"[${nameof(ShipmentController)}] loging called {DateTimeOffset.UtcNow}, method {nameof(Api2)}, result: {result}"
            );

            return Ok(new Api2Response(result));
        }

        /// <summary>
        /// Get total shipping cost based on companies API.
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Amount in USD</response>
        /// <response code="400">Bad request</response> 
        /// <response code="401">If user is not authorized</response>
        [Authorize(Constants.API_XML)]
        [HttpPost("[action]")]
        [Produces("application/xml")]
        [Consumes("application/xml")]
        [ProducesResponseType(typeof(Api3Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Api3(Api3Request request)
        {
            _logger.LogInformation(
                $"[${nameof(ShipmentController)}] loging called {DateTimeOffset.UtcNow}, method {nameof(Api3)}"
            );

            var result = await _companyService.CalculateShipmentAsync(
                request.Source,
                request.Destination,
                request.Packages.Select(s => s.Package)
            );

            _logger.LogInformation(
                $"[${nameof(ShipmentController)}] loging called {DateTimeOffset.UtcNow}, method {nameof(Api3)}, result: {result}"
            );

            return Ok(new Api3Response(result));
        }
    }
}
