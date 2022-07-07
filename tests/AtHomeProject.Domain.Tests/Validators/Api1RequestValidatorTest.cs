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
    public class Api1RequestValidatorTest
    {
        private readonly Api1RequestValidator _validator;

        public Api1RequestValidatorTest() => _validator = new Api1RequestValidator();

        [Fact]
        public void Api1RequestValidator_WhenCall_NoErrorWhenRequestIsValid()
        {
            // Arrange
            var model = new Api1Request
            {
                ContactAddress = "ContactAddress",
                WharehouseAddress = "WharehouseAddress",
                Packages = new List<PackageDimensionModel>
                {
                    new()
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
        public void Api1RequestValidator_WhenCall_HasErrorWhenRequestIsInvalid()
        {
            // Arrange
            var model = new Api1Request();

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldHaveValidationErrorFor(response => response.ContactAddress);
            result.ShouldHaveValidationErrorFor(response => response.WharehouseAddress);
            result.ShouldHaveValidationErrorFor(response => response.Packages);
        }
    }
}
