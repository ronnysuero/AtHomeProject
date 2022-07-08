using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Request;
using AtHomeProject.Domain.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AtHomeProject.Domain.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class Api3RequestValidatorTest
    {
        private readonly Api3RequestValidator _validator;

        public Api3RequestValidatorTest() => _validator = new Api3RequestValidator();

        [Fact]
        public void Api3RequestValidator_WhenCall_NoErrorWhenRequestIsValid()
        {
            // Arrange
            var model = new Api3Request
            {
                Source = "Source",
                Destination = "Destination",
                Packages = new[]
                {
                    new PackageDimension
                    {
                        Height = 1,
                        Weight = 2,
                        Width = 3
                    }
                }
            };

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldNotHaveValidationErrorFor(response => response);
        }

        [Fact]
        public void Api3RequestValidator_WhenCall_HasErrorWhenRequestIsInvalid()
        {
            // Arrange
            var model = new Api3Request();

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldHaveValidationErrorFor(response => response.Source);
            result.ShouldHaveValidationErrorFor(response => response.Destination);
            result.ShouldHaveValidationErrorFor(response => response.Packages);
        }
    }
}
