using System.Diagnostics.CodeAnalysis;
using AtHomeProject.Domain.Models.Auth;
using AtHomeProject.Domain.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace AtHomeProject.Domain.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class UserModelValidatorTest
    {
        private readonly UserModelValidator _validator;

        public UserModelValidatorTest() => _validator = new UserModelValidator();

        [Fact]
        public void UserModelValidator_WhenCall_NoErrorWhenUserIsValid()
        {
            // Arrange
            var model = new UserModel
            {
                UserName = "UserName",
                Password = "Password"
            };

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldNotHaveValidationErrorFor(response => response);
        }

        [Fact]
        public void UserModelValidator_WhenCall_HasErrorWhenUserIsInvalid()
        {
            // Arrange
            var model = new UserModel();

            // Assert
            var result = _validator.TestValidate(model);

            // Act
            result.ShouldHaveValidationErrorFor(response => response.UserName);
            result.ShouldHaveValidationErrorFor(response => response.Password);
        }
    }
}
