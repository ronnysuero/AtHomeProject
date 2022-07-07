using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AtHomeProject.Data;
using AtHomeProject.Data.Tests;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Semver;
using Xunit;

namespace AtHomeProject.Domain.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class SensorInputServiceTests : IClassFixture<SharedDatabaseFixture>
    {
        public SensorInputServiceTests(SharedDatabaseFixture sharedDatabaseFixture) =>
            SharedDatabaseFixture = sharedDatabaseFixture;

        private SharedDatabaseFixture SharedDatabaseFixture { get; }

        [Fact]
        public void SensorInputService_Should_ThrowsArgumentNullExceptionWhenParamsAreNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                // Act
                new SensorInputService(null, null)
            );
        }

        [Fact]
        public async Task InsertRangeAsync_Should_SaveAllRecords()
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
            var service = new SensorInputService(new Mapper(mapperConfig), unitOfWork);

            await unitOfWork.Device.InsertRangeAsync(devices.ToArray());
            await unitOfWork.SaveAsync();

            // Act
            await service.InsertRangeAsync(sensors);

            var sensorsResult = await unitOfWork.SensorInput.GetAsync();

            // Assert
            Assert.Equal(sensors.Count, sensorsResult.Count());
        }

        [Fact]
        public async Task InsertRangeAsync_Should_ThrowsExceptionWhenSerialNumbersDoesntExists()
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
            var service = new SensorInputService(new Mapper(mapperConfig), unitOfWork);

            // Act
            await Assert.ThrowsAsync<DbUpdateException>(() => service.InsertRangeAsync(sensors));
        }

        [Fact]
        public async Task GetPagedAsync_Should_ReturnListOfSensorInputs()
        {
            // Arrange
            var device = new Device
            {
                SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                SecretKey = "fff754c711b34ccd9bf1547f2ea96049",
                FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
            };

            var sensors = new List<SensorInput>()
            {
                new()
                {
                    SerialNumber = device.SerialNumber,
                    DeviceRecorded = DateTime.UtcNow,
                    HealthStatus = "needs_service",
                    Humidity = 22.12,
                    CarbonMonoxide = 4.23,
                    Temperature = 16.47
                },
                new()
                {
                    SerialNumber = device.SerialNumber,
                    DeviceRecorded = DateTime.UtcNow,
                    HealthStatus = "needs_filter",
                    Humidity = 21.87,
                    CarbonMonoxide = 4.30,
                    Temperature = 16.42
                },
                new()
                {
                    SerialNumber = device.SerialNumber,
                    DeviceRecorded = DateTime.UtcNow,
                    HealthStatus = "OK",
                    Humidity = 22.02,
                    CarbonMonoxide = 5.54,
                    Temperature = 16.54
                }
            };

            var mapperConfig = new MapperConfiguration(conf => conf.AddProfile<AutoMapper>());
            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());

            await unitOfWork.Device.InsertAsync(device);
            await unitOfWork.SensorInput.InsertRangeAsync(sensors.ToArray());
            await unitOfWork.SaveAsync();

            var service = new SensorInputService(new Mapper(mapperConfig), unitOfWork);
            const int pageSize = 2;

            // Act
            var sensorsPagedList = await service.GetPagedAsync(
                new()
                {
                    Page = 1,
                    PageSize = pageSize,
                    SerialNumber = device.SerialNumber
                }
            );

            // Assert
            Assert.Equal(sensors.Count, sensorsPagedList.RowCount);
            Assert.Equal(pageSize, sensorsPagedList.Results.Count);
        }

        [Fact]
        public async Task GetPagedAsync_Should_ReturnListOfSensorInputsWhenDateRangeIsEspecified()
        {
            // Arrange
            var device = new Device
            {
                SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                SecretKey = "fff754c711b34ccd9bf1547f2ea96049",
                FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
            };

            var sensors = new List<SensorInput>()
            {
                new()
                {
                    SerialNumber = device.SerialNumber,
                    DeviceRecorded = new DateTime(2019, 1, 1),
                    HealthStatus = "needs_service",
                    Humidity = 22.12,
                    CarbonMonoxide = 4.23,
                    Temperature = 16.47
                },
                new()
                {
                    SerialNumber = device.SerialNumber,
                    DeviceRecorded = new DateTime(2019, 1, 2),
                    HealthStatus = "needs_filter",
                    Humidity = 21.87,
                    CarbonMonoxide = 4.30,
                    Temperature = 16.42
                },
                new()
                {
                    SerialNumber = device.SerialNumber,
                    DeviceRecorded = new DateTime(2019, 1, 3),
                    HealthStatus = "OK",
                    Humidity = 22.02,
                    CarbonMonoxide = 5.54,
                    Temperature = 16.54
                }
            };

            var mapperConfig = new MapperConfiguration(conf => conf.AddProfile<AutoMapper>());
            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());

            await unitOfWork.Device.InsertAsync(device);
            await unitOfWork.SensorInput.InsertRangeAsync(sensors.ToArray());
            await unitOfWork.SaveAsync();

            var service = new SensorInputService(new Mapper(mapperConfig), unitOfWork);
            const int pageSize = 1;

            // Act
            var sensorsPagedList = await service.GetPagedAsync(
                new()
                {
                    Page = 1,
                    PageSize = pageSize,
                    SerialNumber = device.SerialNumber,
                    From = new DateTime(2019, 1, 1),
                    To = new DateTime(2019, 1, 2)
                }
            );

            // Assert
            Assert.Equal(2, sensorsPagedList.RowCount);
            Assert.Equal(pageSize, sensorsPagedList.Results.Count);
        }
    }
}
