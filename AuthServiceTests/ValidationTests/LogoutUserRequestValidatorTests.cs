namespace AuthServiceTests.ValidationTests;

public class LogoutUserRequestValidatorTests
{
    private readonly LogoutUserRequestValidator _validator = new();

    [Fact]
    public void LogoutUserRequestValidator_ShouldPassValidation_WhenDataIsValid()
    {
        // Arrange
        var request = new LogoutUserRequest(Guid.NewGuid(), "ValidRefreshToken123");

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null, "ValidRefreshToken123")]
    [InlineData("00000000-0000-0000-0000-000000000000", "ValidRefreshToken123")]
    [InlineData("b3f0e2e6-4d2a-4f85-91f8-c965c2e8d3b2", "")]
    [InlineData("b3f0e2e6-4d2a-4f85-91f8-c965c2e8d3b2", null)]
    [InlineData("b3f0e2e6-4d2a-4f85-91f8-c965c2e8d3b2", "  ")]
    public void LogoutUserRequestValidator_ShouldFailValidation_WhenDataIsInvalid(string? userId, string? refreshToken)
    {
        // Arrange
        var id = string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        var request = new LogoutUserRequest(id, refreshToken);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null, "ValidRefreshToken123", "UserId must not be empty.")]
    [InlineData("00000000-0000-0000-0000-000000000000", "ValidRefreshToken123", "UserId must not be empty.")]
    [InlineData("b3f0e2e6-4d2a-4f85-91f8-c965c2e8d3b2", "", "RefreshToken is required")]
    [InlineData("b3f0e2e6-4d2a-4f85-91f8-c965c2e8d3b2", null, "RefreshToken is required")]
    [InlineData("b3f0e2e6-4d2a-4f85-91f8-c965c2e8d3b2", "  ", "RefreshToken is required")]
    public void LogoutUserRequestValidator_ShouldReturnCorrectErrorMessage(string? userId, string? refreshToken, string expectedMessage)
    {
        // Arrange
        var request = new LogoutUserRequest(Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : Guid.Empty, refreshToken);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.Should().Contain(e => e.ErrorMessage == expectedMessage);
    }
}