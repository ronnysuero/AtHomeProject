using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Services;
using Xunit;

namespace AtHomeProject.Domain.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class UspsFakeServiceTests
    {
        [Fact]
        public async Task GetRateAsync_Should_GetTotalAmount()
        {
            // Arrange
            var service = new UspsFakeService();

            // Act
            var result = await service.GetRateAsync("", "", new List<PackageDimension>());

            // Assert
            Assert.True(result.Total > 0);
            Assert.NotNull(result.Company);
        }
    }
}
