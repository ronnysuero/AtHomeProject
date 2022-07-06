using System;
using System.Net;
using System.Threading.Tasks;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Pagination;
using AtHomeProject.Web.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AtHomeProject.Web.Controllers
{
    [ApiController]
    [Route("v1/")]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IDeviceService _service;

        public DeviceController(ILogger<DeviceController> logger, IDeviceService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// List recently registered devices sorted by registration date. Requires a SmartAC valid token.
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">List of devices sorted by last registration</response>
        /// <response code="400">Bad request</response> 
        /// <response code="401">If user is not authorized</response>
        [Authorize(Role.User)]
        [HttpPost("[action]")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PagedResult<DeviceListModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Api1(PaginationRequest request)
        {
            _logger.LogInformation(
                $"[${nameof(SensorInputController)}] loging called {DateTimeOffset.UtcNow}, GetPage({request.Page}, {request.PageSize})"
            );

            var results = await _service.GetPagedAsync(request);

            _logger.LogInformation(
                $"[${nameof(SensorInputController)}] loging called {DateTimeOffset.UtcNow}, GetPage({request.Page}, {request.PageSize}), total records: {results.RowCount}"
            );

            return Ok(results);
        }
        
        [Authorize(Role.User)]
        [HttpPost("[action]")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PagedResult<DeviceListModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Api2(PaginationRequest request)
        {
            _logger.LogInformation(
                $"[${nameof(SensorInputController)}] loging called {DateTimeOffset.UtcNow}, GetPage({request.Page}, {request.PageSize})"
            );

            var results = await _service.GetPagedAsync(request);

            _logger.LogInformation(
                $"[${nameof(SensorInputController)}] loging called {DateTimeOffset.UtcNow}, GetPage({request.Page}, {request.PageSize}), total records: {results.RowCount}"
            );

            return Ok(results);
        }
        
        [Authorize(Role.User)]
        [HttpPost("[action]")]
        [Produces("application/xml")]
        [Consumes("application/xml")]
        [ProducesResponseType(typeof(PagedResult<DeviceListModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Api3([FromBody] PaginationRequest request)
        {
            _logger.LogInformation(
                $"[${nameof(SensorInputController)}] loging called {DateTimeOffset.UtcNow}, GetPage({request.Page}, {request.PageSize})"
            );

            var results = await _service.GetPagedAsync(request);

            _logger.LogInformation(
                $"[${nameof(SensorInputController)}] loging called {DateTimeOffset.UtcNow}, GetPage({request.Page}, {request.PageSize}), total records: {results.RowCount}"
            );

            return Ok(results);
        }
    }
}
