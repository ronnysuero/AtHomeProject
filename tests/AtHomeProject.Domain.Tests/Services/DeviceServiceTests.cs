using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AtHomeProject.Data;
using AtHomeProject.Data.Tests;
using AutoMapper;
using Semver;
using Xunit;

namespace AtHomeProject.Domain.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class DeviceServiceTests : IClassFixture<SharedDatabaseFixture>
    {
        public DeviceServiceTests(SharedDatabaseFixture sharedDatabaseFixture) =>
            SharedDatabaseFixture = sharedDatabaseFixture;

        private SharedDatabaseFixture SharedDatabaseFixture { get; }

        [Fact]
        public void DeviceService_Should_ThrowsArgumentNullExceptionWhenParamsAreNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                // Act
                new DeviceService(null, null)
            );
        }

        [Fact]
        public async Task InsertRangeAsync_Should_SaveDevices()
        {
            // Arrange
            var devices = new List<DeviceModel>()
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
                    SecretKey = "fff754c711b34ccd9bf1547f2ea96049",
                    FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
                }
            };

            var mapperConfig = new MapperConfiguration(conf => conf.AddProfile<AutoMapper>());

            var service = new DeviceService(
                new Mapper(mapperConfig),
                new UnitOfWork(SharedDatabaseFixture.CreateContext())
            );

            // Act
            await service.InsertRangeAsync(devices);

            var deviceResult = await service.GetAsync();

            // Assert
            Assert.Equal(devices.Count, deviceResult.Count());
        }

        [Fact]
        public async Task GetPagedAsync_Should_ReturnListOfDevices()
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

            var mapperConfig = new MapperConfiguration(conf => conf.AddProfile<AutoMapper>());
            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());

            await unitOfWork.Device.InsertRangeAsync(devices.ToArray());
            await unitOfWork.SaveAsync();

            var service = new DeviceService(new Mapper(mapperConfig), unitOfWork);

            // Act
            var devicesPagedList = await service.GetPagedAsync(
                new()
                {
                    Page = 1,
                    PageSize = 2
                }
            );

            // Assert
            Assert.Equal(2, devicesPagedList.Results.Count);
            Assert.Equal(devices.Count, devicesPagedList.RowCount);
        }

        [Fact]
        public async Task GetBySerialNumberAsync_Should_ReturnDeviceBySerialNumber()
        {
            // Arrange
            var device = new Device()
            {
                SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                SecretKey = "fff754c711b34ccd9bf1547f2ea96049",
                FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
            };

            var mapperConfig = new MapperConfiguration(conf => conf.AddProfile<AutoMapper>());
            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());

            await unitOfWork.Device.InsertAsync(device);
            await unitOfWork.SaveAsync();

            var service = new DeviceService(new Mapper(mapperConfig), unitOfWork);

            // Act
            var deviceResult = await service.GetBySerialNumberAsync(device.SerialNumber);

            // Assert
            Assert.Equal(device.SerialNumber, deviceResult.SerialNumber);
        }
    }
}
