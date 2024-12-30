namespace AuthServiceTests.ValidationTests;

public class LoginUserRequestValidatorTests
{
    private readonly LoginUserRequestValidator _validator = new();

    [Theory]
    [InlineData("email@mail.com", "password123-R")]
    [InlineData("user@example.com", "P@ssw0rd!")]
    [InlineData("valid.user+alias@mail.com", "SecurePassword123")]
    public void LoginUserRequestValidator_ShouldPassValidation(string email, string password)
    {
        // Arrange
        var request = new LoginUserRequest(email, password);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "password123-R")]
    [InlineData(null, "password123-R")]
    [InlineData("invalid-email", "password123-R")]
    [InlineData("email@mail.com", "")]
    [InlineData("email@mail.com", null)]
    [InlineData("email@mail.com", "     ")]
    public void LoginUserRequestValidator_ShouldFailValidation_WhenEmailOrPasswordIsInvalid(string email, string password)
    {
        // Arrange
        var request = new LoginUserRequest(email, password);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "password123-R", "Email is required.")]
    [InlineData(null, "password123-R", "Email is required.")]
    [InlineData("invalid-email", "password123-R", "Invalid email format.")]
    [InlineData("email@mail.com", "", "Password is required.")]
    [InlineData("email@mail.com", null, "Password is required.")]
    public void LoginUserRequestValidator_ShouldReturnCorrectErrorMessage(string email, string password, string expectedMessage)
    {
        // Arrange
        var request = new LoginUserRequest(email, password);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.Should().Contain(e => e.ErrorMessage == expectedMessage);
    }
}
