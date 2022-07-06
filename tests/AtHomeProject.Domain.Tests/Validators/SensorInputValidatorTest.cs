using System.Diagnostics.CodeAnalysis;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AtHomeProject.Domain.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class SensorInputValidatorTest
    {
        private readonly SensorInputValidator _validator;

        public SensorInputValidatorTest()
        {
            _validator = new SensorInputValidator();
        }

        [Fact]
        public void SensorInputValidator_WhenCall_NoErrorWhenSensorInputModelIsValid()
        {
            // Arrange
            var model = new SensorInputModel
            {
                Temperature = 12,
                Humidity = 23,
                SerialNumber = "123123121123424",
                HealthStatus = "ok"
            };

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldNotHaveValidationErrorFor(sensor => sensor);
        }

        [Fact]
        public void SensorInputValidator_WhenCall_HasErrorWhenSensorInputModelIsInvalid()
        {
            // Arrange
            var model = new SensorInputModel
            {
                Temperature = 12,
                Humidity = 23,
                SerialNumber = "123123121123424",
                HealthStatus = "whatever"
            };

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldHaveValidationErrorFor(sensor => sensor.HealthStatus);
        }
    }
}
