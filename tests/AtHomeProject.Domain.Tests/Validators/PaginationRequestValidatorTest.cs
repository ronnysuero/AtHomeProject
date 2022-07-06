using System.Diagnostics.CodeAnalysis;
using AtHomeProject.Domain.Models.Pagination;
using AtHomeProject.Domain.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AtHomeProject.Domain.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class PaginationRequestValidatorTest
    {
        private readonly PaginationRequestValidator _validator;

        public PaginationRequestValidatorTest()
        {
            _validator = new PaginationRequestValidator();
        }

        [Fact]
        public void PaginationRequestValidator_WhenCall_NoErrorWhenPaginationRequestIsValid()
        {
            // Arrange
            var model = new PaginationRequest
            {
                Page = 1,
                PageSize = 2
            };

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldNotHaveValidationErrorFor(sensor => sensor);
        }

        [Fact]
        public void PaginationRequestValidator_WhenCall_HasErrorWhenPaginationRequestIsInvalid()
        {
            // Arrange
            var model = new PaginationRequest
            {
                Page = -1,
                PageSize = -2
            };

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldHaveValidationErrorFor(sensor => sensor.Page);
            result.ShouldHaveValidationErrorFor(sensor => sensor.PageSize);
        }
    }
}
