using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Pagination;
using AtHomeProject.Domain.Services;
using AtHomeProject.Web.Auth;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// ReSharper disable PossibleMultipleEnumeration
namespace AtHomeProject.Web.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class SensorInputController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISensorInputService _service;

        public SensorInputController(ILogger<SensorInputController> logger, ISensorInputService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Register batches of sensor inputs. Requires a SmartAC valid token.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///        [{
        ///            "serialNumber": "ea9c98ed90df4d2686d1b57264e8159e",
        ///            "deviceRecorded": "2022-05-11T16:32:47.643Z",
        ///            "healthStatus": "needs_service",
        ///            "humidity": 22.12,
        ///            "carbonMonoxide": 4.23,
        ///            "temperature": 16.47
        ///        },
        ///        {
        ///            "serialNumber": "02488664b3dd433ba0ab64ba84b9539c",
        ///            "deviceRecorded": "2022-05-11T16:32:47.643Z",
        ///            "healthStatus": "needs_filter",
        ///            "humidity": 21.87,
        ///            "carbonMonoxide": 4.30,
        ///            "temperature": 16.42
        ///        },
        ///        {
        ///            "serialNumber": "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
        ///            "deviceRecorded": "2022-05-11T16:32:47.643Z",
        ///            "healthStatus": "OK",
        ///            "humidity": 22.02,
        ///            "carbonMonoxide": 5.54,
        ///            "temperature": 16.54
        ///        }]
        ///
        /// </remarks>
        /// <param name="models"></param>
        /// <response code="200">All sensor inputs records are saved</response>
        /// <response code="400">Bad request</response> 
        /// <response code="401">If device is not authorized</response>
        [Authorize(Role.Device)]
        [HttpPost("Add/SensorInput/Batch")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> AddSensorInputBatches(IEnumerable<SensorInputModel> models)
        {
            _logger.LogInformation(
                $"[${nameof(SensorInputController)}] loging called {DateTimeOffset.UtcNow}, total records: {models.Count()}"
            );

            await _service.InsertRangeAsync(models);

            // The alerts will be processed by the background service
            BackgroundJob.Enqueue<SensorAlertService>(job => job.ProcessSensorsInputAsync(models.ToList()));

            _logger.LogInformation(
                $"[${nameof(SensorInputController)}] loging called {DateTimeOffset.UtcNow}, sensors batches sucessfully saved, total records: {models.Count()}"
            );

            return Ok();
        }

        /// <summary>
        /// List sensor readings for a device by date range. Requires a User valid token.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///        {
        ///             "page": 1,
        ///             "pageSize": 10,
        ///             "serialNumber": "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
        ///             "from": null,
        ///             "to": null
        ///        }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <response code="200">All sensor inputs records</response>
        /// <response code="400">Bad request</response> 
        /// <response code="401">If user is not authorized</response>
        [Authorize(Role.User)]
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(PagedResult<SensorInputListModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetPage(SensorInputRequest request)
        {
            _logger.LogInformation(
                $"[${nameof(SensorInputController)}] loging called {DateTimeOffset.UtcNow}, GetPage({request.Page}, {request.PageSize}"
            );

            var results = await _service.GetPagedAsync(request);

            _logger.LogInformation(
                $"[${nameof(SensorInputController)}] loging called {DateTimeOffset.UtcNow}, total records: {results.RowCount}"
            );

            return Ok(results);
        }
    }
}
