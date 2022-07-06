using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtHomeProject.Data.Entities;
using AtHomeProject.Data.Enum;
using AtHomeProject.Data.Interfaces;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

// ReSharper disable CognitiveComplexity
namespace AtHomeProject.Domain.Services
{
    public class SensorAlertService : ISensorAlertService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly List<SensorAlertModel> _alerts = new();

        public SensorAlertService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<SensorAlertService> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private void AddAlert(SensorInputModel model, double value, string description)
        {
            _alerts.Add(
                new SensorAlertModel
                {
                    Value = value,
                    Description = description,
                    DeviceRecorded = model.DeviceRecorded,
                    SerialNumber = model.SerialNumber,
                    ResolvedState = ResolvedState.New,
                    ViewState = ViewState.New
                }
            );
        }

        private async Task SaveChangesAsync()
        {
            if (_alerts.Count == 0)
                return;

            _logger.LogInformation(
                $"[${nameof(SensorAlertService)}] loging called {DateTimeOffset.UtcNow}, amount of alerts generated {_alerts.Count}"
            );

            var entities = _mapper.Map<IEnumerable<SensorAlert>>(_alerts).ToArray();

            await _unitOfWork.SensorAlert.InsertRangeAsync(entities);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation(
                $"[${nameof(SensorAlertService)}] loging called {DateTimeOffset.UtcNow}, alerts saved sucessfully"
            );
        }

        public async Task ProcessSensorsInputAsync(List<SensorInputModel> models)
        {
            _logger.LogInformation(
                $"[${nameof(SensorAlertService)}] loging called {DateTimeOffset.UtcNow}, starting processing {models.Count} sensors"
            );

            foreach (var model in models)
            {
                if (model.Temperature is < -30 or > 100)
                    AddAlert(model, model.Temperature, "Sensor temperature has value out of range");

                if (model.Humidity is < 0 or > 100)
                    AddAlert(model, model.Humidity, "Sensor humidity has value out of range");

                if (model.CarbonMonoxide is < 0 or > 1000)
                    AddAlert(model, model.CarbonMonoxide, "Sensor carbon monoxide has value out of range");

                if (model.CarbonMonoxide is > 9)
                    AddAlert(model, model.CarbonMonoxide, "CO value has exceeded danger limit");

                if (!model.HealthStatus.ToLower().Equals("ok"))
                    AddAlert(model, model.CarbonMonoxide, "Device is reporting health problem");
            }

            await SaveChangesAsync();
        }
    }
}
