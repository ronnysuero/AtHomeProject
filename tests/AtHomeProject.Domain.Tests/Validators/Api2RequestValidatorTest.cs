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
    public class Api2RequestValidatorTest
    {
        private readonly Api2RequestValidator _validator;

        public Api2RequestValidatorTest() => _validator = new Api2RequestValidator();

        [Fact]
        public void Api2RequestValidator_WhenCall_NoErrorWhenRequestIsValid()
        {
            // Arrange
            var model = new Api2Request
            {
                Consignee = "Consignee",
                Consignor = "Consignor",
                Cartons = new List<PackageDimension>
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
        public void Api2RequestValidator_WhenCall_HasErrorWhenRequestIsInvalid()
        {
            // Arrange
            var model = new Api2Request();

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldHaveValidationErrorFor(response => response.Consignee);
            result.ShouldHaveValidationErrorFor(response => response.Consignor);
            result.ShouldHaveValidationErrorFor(response => response.Cartons);
        }
    }
}
