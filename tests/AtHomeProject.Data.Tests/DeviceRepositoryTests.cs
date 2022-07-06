using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AtHomeProject.Data.Entities;
using Semver;
using Xunit;

namespace AtHomeProject.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public class DeviceRepositoryTests : IClassFixture<SharedDatabaseFixture>
    {
        public DeviceRepositoryTests(SharedDatabaseFixture sharedDatabaseFixture) =>
            SharedDatabaseFixture = sharedDatabaseFixture;

        private SharedDatabaseFixture SharedDatabaseFixture { get; }

        [Fact]
        public void DeviceRepository_Should_ThrowsArgumentNullExceptionWhenContextIsNull()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
                // Act
                new Repository<Device>(null)
            );
        }

        [Fact]
        public async Task InsertAsync_Should_SaveDevice()
        {
            // Arrange
            var device = new Device
            {
                SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                SecretKey = "fff754c711b34ccd9bf1547f2ea96049",
                FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
            };

            await using var context = SharedDatabaseFixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);

            // Act
            await unitOfWork.Device.InsertAsync(device);
            await unitOfWork.SaveAsync();

            var deviceResult = await unitOfWork.Device.GetByKeyAsync(device.SerialNumber);

            // Assert
            Assert.Equal(device.SerialNumber, deviceResult.SerialNumber);
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteDevice()
        {
            // Arrange
            var device = new Device
            {
                SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                SecretKey = "fff754c711b34ccd9bf1547f2ea96049",
                FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
            };

            await using var context = SharedDatabaseFixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);

            await unitOfWork.Device.InsertAsync(device);
            await unitOfWork.SaveAsync();

            // Act
            await unitOfWork.Device.DeleteAsync(device.SerialNumber);
            await unitOfWork.SaveAsync();

            var deviceResult = await unitOfWork.Device.GetByKeyAsync(device.SerialNumber);

            // Assert
            Assert.Null(deviceResult);
        }

        [Fact]
        public async Task GetAsync_Should_ReturnsEmptyCollection()
        {
            // Arrange
            await using var context = SharedDatabaseFixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);

            // Act
            var devices = await unitOfWork.Device.GetAsync();

            // Assert
            Assert.Empty(devices);
        }
    }
}
