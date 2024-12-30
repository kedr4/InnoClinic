namespace AuthServiceTests.ValidationTests;

public class RefreshTokenRequestValidatorTests
{
    private readonly RefreshTokenRequestValidator _validator = new();

    [Fact]
    public void RefreshTokenRequestValidator_ShouldPassValidation_WhenDataIsValid()
    {
        // Arrange
        var request = new RefreshTokenRequest("ValidRefreshToken123", Guid.NewGuid());

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "00000000-0000-0000-0000-000000000000")]
    [InlineData(null, "00000000-0000-0000-0000-000000000000")]
    [InlineData(" ", "00000000-0000-0000-0000-000000000000")]
    [InlineData("ValidRefreshToken123", null)]
    [InlineData("ValidRefreshToken123", "00000000-0000-0000-0000-000000000000")]
    public void RefreshTokenRequestValidator_ShouldFailValidation_WhenDataIsInvalid(string refreshToken, string userId)
    {
        // Arrange
        var request = new RefreshTokenRequest(refreshToken, Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : Guid.Empty);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "00000000-0000-0000-0000-000000000000", "Refresh token is required.")]
    [InlineData(null, "00000000-0000-0000-0000-000000000000", "Refresh token is required.")]
    [InlineData(" ", "00000000-0000-0000-0000-000000000000", "Refresh token is required.")]
    [InlineData("ValidRefreshToken123", null, "User ID is required.")]
    [InlineData("ValidRefreshToken123", "00000000-0000-0000-0000-000000000000", "User ID is required.")]
    public void RefreshTokenRequestValidator_ShouldReturnCorrectErrorMessage(string refreshToken, string userId, string expectedMessage)
    {
        // Arrange
        var request = new RefreshTokenRequest(refreshToken, Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : Guid.Empty);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.Should().Contain(e => e.ErrorMessage == expectedMessage);
    }
}