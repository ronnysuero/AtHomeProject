using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AtHomeProject.Data.Entities;
using Xunit;

namespace AtHomeProject.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public class UserRepositoryTests : IClassFixture<SharedDatabaseFixture>
    {
        public UserRepositoryTests(SharedDatabaseFixture sharedDatabaseFixture) =>
            SharedDatabaseFixture = sharedDatabaseFixture;

        private SharedDatabaseFixture SharedDatabaseFixture { get; }

        [Fact]
        public void UserRepository_Should_ThrowsArgumentNullExceptionWhenContextIsNull()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
                // Act
                new Repository<Users>(null)
            );
        }

        [Fact]
        public async Task InsertAsync_Should_SaveUser()
        {
            // Arrange
            var entity = new Users
            {
                Id = 1,
                Username = "Username",
                Password = "Password"
            };

            await using var context = SharedDatabaseFixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);

            // Act
            await unitOfWork.Users.InsertAsync(entity);
            await unitOfWork.SaveAsync();

            var result = await unitOfWork.Users.GetByKeyAsync(entity.Id);

            // Assert
            Assert.Equal(entity.Id, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteUser()
        {
            // Arrange
            var entity = new Users
            {
                Id = 1,
                Username = "Username",
                Password = "Password"
            };

            await using var context = SharedDatabaseFixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);

            await unitOfWork.Users.InsertAsync(entity);
            await unitOfWork.SaveAsync();

            // Act
            await unitOfWork.Users.DeleteAsync(entity.Id);
            await unitOfWork.SaveAsync();

            var result = await unitOfWork.Users.FindAsync(f => f.Id == entity.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_Should_ReturnsEmptyCollection()
        {
            // Arrange
            await using var context = SharedDatabaseFixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);

            // Act
            var records = await unitOfWork.Users.GetAsync();

            // Assert
            Assert.Empty(records);
        }

        [Fact]
        public async Task InsertRangeAsync_Should_SaveUsers()
        {
            // Arrange
            var entities = new List<Users>
            {
                new()
                {
                    Id = 1,
                    Username = "Username1",
                    Password = "Password1"
                },
                new()
                {
                    Id = 2,
                    Username = "Username2",
                    Password = "Password2"
                }
            };

            await using var context = SharedDatabaseFixture.CreateContext();
            var unitOfWork = new UnitOfWork(context);

            // Act
            await unitOfWork.Users.InsertRangeAsync(entities.ToArray());
            await unitOfWork.SaveAsync();

            var count = await unitOfWork.Users.CountAsync;

            // Assert
            Assert.Equal(entities.Count, count);
        }
    }
}
