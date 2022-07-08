using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AtHomeProject.Data;
using AtHomeProject.Data.Entities;
using AtHomeProject.Data.Tests;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Auth;
using AtHomeProject.Domain.Services;
using Microsoft.Extensions.Options;
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
                new AuthService(null, null)
            );
        }

        [Fact]
        public async Task AuthenticateAsync_Should_GetValidTokenWhenCredentialsAreValid()
        {
            // Arrange
            var user = new Users
            {
                Id = 1,
                Username = "Username",
                Password = "Password"
            };

            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());
            var options = Options.Create(new AppSettings { Secret = "random secret key" });

            await unitOfWork.Users.InsertAsync(user);
            await unitOfWork.SaveAsync();

            var authService = new AuthService(unitOfWork, options);

            // Act
            var result = await authService.AuthenticateAsync(
                new()
                {
                    UserName = "Username",
                    Password = "Password"
                }
            );

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_ReturnsNullWhenCredentialsAreInvalid()
        {
            // Arrange
            var options = Options.Create(new AppSettings { Secret = "random secret key" });
            var unitOfWork = new UnitOfWork(SharedDatabaseFixture.CreateContext());
            var authService = new AuthService(unitOfWork, options);

            // Act
            var result = await authService.AuthenticateAsync(
                new()
                {
                    UserName = "Username",
                    Password = "Password"
                }
            );

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Authenticate_Should_ReturnsNullWhenUserCredentialsAreInvalid()
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
            var result = await authService.AuthenticateAsync(user);

            // Assert
            Assert.Null(result);
        }
    }
}
