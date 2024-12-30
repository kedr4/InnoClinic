namespace AuthServiceTests.ValidationTests;

public class CreateUserRequestValidatorTests
{
    private readonly CreateUserRequestValidator _validator = new();

    [Theory]
    [InlineData("valid.email@mail.com", "Valid123!")]
    [InlineData("test_user@mail.com", "StrongPass123#")]
    [InlineData("example@mail.com", "Password@2023")]
    public void CreateUserRequestValidator_ShouldPassValidation(string email, string password)
    {
        // Arrange  
        var request = new CreateUserRequest(email, password);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Valid123!")]
    [InlineData(null, "Valid123!")]
    [InlineData("invalid-email", "Valid123!")]
    [InlineData("valid.email@mail.com", "")]
    [InlineData("valid.email@mail.com", null)]
    [InlineData("valid.email@mail.com", "short")]
    [InlineData("valid.email@mail.com", "nouppercase123!")]
    [InlineData("valid.email@mail.com", "NOLOWERCASE123!")]
    [InlineData("valid.email@mail.com", "NoDigits!")]
    [InlineData("valid.email@mail.com", "NoSpecial123")]
    public void CreateUserRequestValidator_ShouldFailValidation_WhenEmailOrPasswordIsInvalid(string email, string password)
    {
        // Arrange
        var request = new CreateUserRequest(email, password);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "Valid123!", "Email is required.")]
    [InlineData("invalid-email", "Valid123!", "Invalid email format.")]
    [InlineData("valid.email@mail.com", "", "Password is required.")]
    [InlineData("valid.email@mail.com", "short", "Password must be at least 8 characters long.")]
    [InlineData("valid.email@mail.com", "nouppercase123!", "Password must contain at least one uppercase letter.")]
    [InlineData("valid.email@mail.com", "NOLOWERCASE123!", "Password must contain at least one lowercase letter.")]
    [InlineData("valid.email@mail.com", "NoDigits!", "Password must contain at least one digit.")]
    [InlineData("valid.email@mail.com", "NoSpecial123", "Password must contain at least one special character (e.g., !, @, #, $, etc.).")]
    public void CreateUserRequestValidator_ShouldReturnCorrectErrorMessage(string email, string password, string expectedMessage)
    {
        // Arrange
        var request = new CreateUserRequest(email, password);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.Should().Contain(e => e.ErrorMessage == expectedMessage);
    }
}