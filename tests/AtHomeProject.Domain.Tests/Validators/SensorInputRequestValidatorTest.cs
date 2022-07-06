using System.Diagnostics.CodeAnalysis;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AtHomeProject.Domain.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class SensorInputRequestValidatorTest
    {
        private readonly SensorInputRequestValidator _validator;

        public SensorInputRequestValidatorTest()
        {
            _validator = new SensorInputRequestValidator();
        }

        [Fact]
        public void SensorInputRequestValidator_WhenCall_NoErrorWhenSensorInputRequestIsValid()
        {
            // Arrange
            var model = new SensorInputRequest
            {
                Page = 1,
                PageSize = 2,
                SerialNumber = "12345678"
            };

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldNotHaveValidationErrorFor(sensor => sensor);
        }

        [Fact]
        public void SensorInputRequestValidator_WhenCall_HasErrorWhenSensorInputRequestIsInvalid()
        {
            // Arrange
            var model = new SensorInputRequest
            {
                Page = -1,
                PageSize = -2
            };

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldHaveValidationErrorFor(sensor => sensor.Page);
            result.ShouldHaveValidationErrorFor(sensor => sensor.PageSize);
            result.ShouldHaveValidationErrorFor(sensor => sensor.SerialNumber);
        }
    }
}
