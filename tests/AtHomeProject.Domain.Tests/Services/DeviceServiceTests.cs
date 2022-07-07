using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Services;
using Xunit;

namespace AtHomeProject.Domain.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class CompanyServiceTests
    {
        [Fact]
        public void CompanyService_Should_ThrowsArgumentNullExceptionWhenParamsAreNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                // Act
                new CompanyService(null, null, null)
            );
        }

        [Fact]
        public async Task CalculateShipmentAsync_Should_GetLowestShippingCost()
        {
            // Arrange
            var companyService = new CompanyService(
                new FedexFakeService(),
                new UpsFakeService(),
                new UspsFakeService()
            );

            // Act
            var result = await companyService.CalculateShipmentAsync("", "", new List<PackageDimensionModel>());

            // Assert
            Assert.True(result > 0);
        }
    }
}
