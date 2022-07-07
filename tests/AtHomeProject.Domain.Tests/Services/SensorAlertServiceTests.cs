using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AtHomeProject.Data;
using AtHomeProject.Data.Tests;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Semver;
using Xunit;

namespace AtHomeProject.Domain.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class SensorAlertServiceTests : IClassFixture<SharedDatabaseFixture>
    {
        public SensorAlertServiceTests(SharedDatabaseFixture sharedDatabaseFixture) =>
            SharedDatabaseFixture = sharedDatabaseFixture;

        private SharedDatabaseFixture SharedDatabaseFixture { get; }

        [Fact]
        public void SensorAlertService_Should_ThrowsArgumentNullExceptionWhenParamsAreNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                // Act
                new SensorAlertService(null, null, null)
            );
        }

        [Fact]
        public async Task AnalyzeSensorsInput_Should_SaveAllAlertsRecords()
        {
            // Arrange
            var devices = new List<Device>
            {
                new()
                {
                    SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                    SecretKey = "fff754c711b34ccd9bf1547f2ea96049",
                    FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
                },
                new()
                {
                    SerialNumber = "02488664b3dd433ba0ab64ba84b9539c",
                    SecretKey = "d7c8b47a619c442da8e918b875ea3e5c",
                    FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
                },
                new()
                {
                    SerialNumber = "ea9c98ed90df4d2686d1b57264e8159e",
                    SecretKey = "ad5f8a5dffc542ef8417230f090a4b05",
                    FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
                },
            };

            var sensors = new List<SensorInputModel>
            {
                new()
                {
                    SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                    DeviceRecorded = DateTime.UtcNow,
                    HealthStatus = "OK",
                    Humidity = 22.12,
                    CarbonMonoxide = 9.23,
                    Temperature = 16.47
                },
                new()
                {
                    SerialNumber = "02488664b3dd433ba0ab64ba84b9539c",
                    DeviceRecorded = DateTime.UtcNow,
                    HealthStatus = "needs_service",
                    Humidity = 22.12,
                    CarbonMonoxide = 4.23,
                    Temperature = -66.47
                },
                new()
                {
                    SerialNumber = "ea9c98ed90df4d2686d1b57264e8159e",
                    DeviceRecorded = DateTime.UtcNow,
                    HealthStatus = "needs_filter",
                    Humidity = -22.12,
                    CarbonMonoxide = 4.23,
                    Temperature = 16.47
                }
            };

            const int expectedAlertCount = 5;
            var mapperConfig = new MapperConfiguration(conf => conf.AddProfile<AutoMapper>());
            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());

            var service = new SensorAlertService(
                new Mapper(mapperConfig),
                unitOfWork,
                Substitute.For<ILogger<SensorAlertService>>()
            );

            await unitOfWork.Device.InsertRangeAsync(devices.ToArray());
            await unitOfWork.SaveAsync();

            // Act
            await service.ProcessSensorsInputAsync(sensors);

            var sensorsAlertResult = await unitOfWork.SensorAlert.GetAsync();

            // Assert
            Assert.Equal(expectedAlertCount, sensorsAlertResult.Count());
        }

        [Fact]
        public void AnalyzeSensorsInput_Should_ThrowsExceptionWhenSerialNumbersDoesntExists()
        {
            // Arrange
            var sensors = new List<SensorInputModel>
            {
                new()
                {
                    SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                    DeviceRecorded = DateTime.UtcNow,
                    HealthStatus = "OK",
                    Humidity = 22.12,
                    CarbonMonoxide = 4.23,
                    Temperature = 16.47
                },
                new()
                {
                    SerialNumber = "02488664b3dd433ba0ab64ba84b9539c",
                    DeviceRecorded = DateTime.UtcNow,
                    HealthStatus = "needs_service",
                    Humidity = 22.12,
                    CarbonMonoxide = 4.23,
                    Temperature = 16.47
                },
                new()
                {
                    SerialNumber = "ea9c98ed90df4d2686d1b57264e8159e",
                    DeviceRecorded = DateTime.UtcNow,
                    HealthStatus = "needs_filter",
                    Humidity = 22.12,
                    CarbonMonoxide = 4.23,
                    Temperature = 16.47
                }
            };

            var mapperConfig = new MapperConfiguration(conf => conf.AddProfile<AutoMapper>());
            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());

            var service = new SensorAlertService(
                new Mapper(mapperConfig),
                unitOfWork,
                Substitute.For<ILogger<SensorAlertService>>()
            );

            // Act
            Assert.ThrowsAsync<DbUpdateException>(async () => await service.ProcessSensorsInputAsync(sensors));
        }
    }
}
