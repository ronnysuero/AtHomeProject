using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AtHomeProject.Data;
using AtHomeProject.Data.Tests;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Auth;
using AtHomeProject.Domain.Services;
using AutoMapper;
using Microsoft.Extensions.Options;
using Semver;
using Xunit;

namespace AtHomeProject.Domain.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class AuthServiceTests : IClassFixture<SharedDatabaseFixture>
    {
        public AuthServiceTests(SharedDatabaseFixture sharedDatabaseFixture) =>
            SharedDatabaseFixture = sharedDatabaseFixture;

        private SharedDatabaseFixture SharedDatabaseFixture { get; }

        [Fact]
        public void AuthService_Should_ThrowsArgumentNullExceptionWhenParamsAreNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                // Act
                new DeviceService(null, null)
            );
        }

        [Fact]
        public async Task AuthenticateAsync_Should_GetValidTokenWhenDeviceCredentialsAreValid()
        {
            // Arrange
            var device = new DeviceModel
            {
                SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                SecretKey = "fff754c711b34ccd9bf1547f2ea96049",
                FirmwareVersion = SemVersion.Parse("1.0.0", SemVersionStyles.Strict)
            };

            var mapperConfig = new MapperConfiguration(conf => conf.AddProfile<AutoMapper>());
            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());
            var deviceService = new DeviceService(new Mapper(mapperConfig), unitOfWork);


            await deviceService.InsertAsync(device);

            var options = Options.Create(new AppSettings
            {
                Secret = "random secret key"
            });

            var authService = new AuthService(unitOfWork, options);

            // Act
            var result = await authService.AuthenticateAsync(
                new DeviceAuthenticateRequest
                {
                    SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                    SecretKey = "fff754c711b34ccd9bf1547f2ea96049"
                }
            );

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_ReturnsNullWhenDeviceCredentialsAreInvalid()
        {
            // Arrange
            var options = Options.Create(new AppSettings
            {
                Secret = "random secret key"
            });

            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());
            var authService = new AuthService(unitOfWork, options);

            // Act
            var result = await authService.AuthenticateAsync(
                new DeviceAuthenticateRequest
                {
                    SerialNumber = "0d0f40f0acb74bf0958c6c6c2a7e6f1f",
                    SecretKey = "fff754c711b34ccd9bf1547f2ea96049"
                }
            );

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_Should_GetValidTokenWhenUserCredentialsAreValid()
        {
            // Arrange
            var user = new UserModel
            {
                UserName = "john-doe",
                Password = "123456"
            };

            var options = Options.Create(new AppSettings
            {
                Secret = "random secret key",
                DefaultCredentials = new UserModel
                {
                    UserName = "john-doe",
                    Password = "123456"
                }
            });

            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());
            var authService = new AuthService(unitOfWork, options);

            // Act
            var result = authService.Authenticate(user);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
        }

        [Fact]
        public void Authenticate_Should_ReturnsNullWhenUserCredentialsAreInvalid()
        {
            // Arrange
            var user = new UserModel
            {
                UserName = "john-doe",
                Password = "123456"
            };

            var options = Options.Create(new AppSettings
            {
                Secret = "random secret key"
            });

            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());
            var authService = new AuthService(unitOfWork, options);

            // Act
            var result = authService.Authenticate(user);

            // Assert
            Assert.Null(result);
        }
    }
}
