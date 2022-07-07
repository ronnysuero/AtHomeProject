using System.Diagnostics.CodeAnalysis;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AtHomeProject.Domain.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class CartonDimensionModelValidatorTest
    {
        private readonly CartonDimensionModelValidator _validator;

        public CartonDimensionModelValidatorTest() => _validator = new CartonDimensionModelValidator();

        [Fact]
        public void PackageDimensionModelValidator_WhenCall_NoErrorWhenPackageDimensionIsValid()
        {
            // Arrange
            var model = new CartonDimensionModel
            {
                Package = new()
                {
                    Height = 1,
                    Weight = 2,
                    Width = 3
                }
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
            var model = new CartonDimensionModel();

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldHaveValidationErrorFor(response => response.Package);
        }
    }
}
