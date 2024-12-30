namespace AuthServiceTests.ValidationTests;

public class ConfirmMailRequestValidatorTests
{
    private readonly ConfirmMailRequestValidator _validator = new ConfirmMailRequestValidator();

    [Theory]
    [InlineData("123e4567-e89b-12d3-a456-426614174000", "valid-token-123")]
    [InlineData("4b9cd765-89ec-4d4f-9a64-d6e4e775a78c", "token-!@#$%^")]
    public void ConfirmMailRequestValidator_ShouldPassValidation(string userId, string token)
    {
        // Arrange
        var request = new ConfirmMailRequest(Guid.Parse(userId), token);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "valid-token-123")]
    [InlineData(null, "valid-token-123")]
    [InlineData("00000000-0000-0000-0000-000000000000", "valid-token-123")]
    [InlineData("123e4567-e89b-12d3-a456-426614174000", "")]
    [InlineData("123e4567-e89b-12d3-a456-426614174000", null)]
    [InlineData("123e4567-e89b-12d3-a456-426614174000", "   ")]
    public void ConfirmMailRequestValidator_ShouldFailValidation_WhenUserIdOrTokenIsInvalid(string userId, string token)
    {
        // Arrange
        var id = string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        var request = new ConfirmMailRequest(id, token);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "valid-token-123", "UserId is required")]
    [InlineData(null, "valid-token-123", "UserId is required")]
    [InlineData("123e4567-e89b-12d3-a456-426614174000", "", "Code is required")]
    [InlineData("123e4567-e89b-12d3-a456-426614174000", null, "Code is required")]
    public void ConfirmMailRequestValidator_ShouldReturnCorrectErrorMessage(string? userId, string? token, string expectedMessage)
    {
        // Arrange
        var id = string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        var request = new ConfirmMailRequest(id, token);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.Errors.Should().Contain(e => e.ErrorMessage == expectedMessage);
    }
}
