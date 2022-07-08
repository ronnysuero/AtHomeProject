using System.Diagnostics.CodeAnalysis;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AtHomeProject.Domain.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class PackageDimensionModelValidatorTest
    {
        private readonly PackageDimensionValidator _validator;

        public PackageDimensionModelValidatorTest() => _validator = new PackageDimensionValidator();

        [Fact]
        public void PackageDimensionModelValidator_WhenCall_NoErrorWhenPackageDimensionIsValid()
        {
            // Arrange
            var model = new PackageDimension
            {
                Height = 1,
                Weight = 2,
                Width = 3
            };

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldNotHaveValidationErrorFor(response => response);
        }

        [Fact]
        public void PackageDimensionModelValidator_WhenCall_HasErrorWhenPackageDimensionIsInvalid()
        {
            // Arrange
            var model = new PackageDimension();

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldHaveValidationErrorFor(response => response.Height);
            result.ShouldHaveValidationErrorFor(response => response.Weight);
            result.ShouldHaveValidationErrorFor(response => response.Width);
        }
    }
}
