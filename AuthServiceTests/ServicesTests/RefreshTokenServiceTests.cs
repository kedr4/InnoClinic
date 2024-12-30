using Application.Exceptions;
using Domain.Models;

namespace AuthServiceTests.ServicesTests;

public class RefreshTokenServiceTests : TestFixture
{
    [Fact]
    public async Task CreateUserRefreshTokenAsync_ShouldCreateAndReturnRefreshToken()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await RefreshTokenService.CreateUserRefreshTokenAsync(DefaultUser, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(DefaultUser.Id);
        RefreshTokenRepositoryMock.Verify(r => r.CreateTokenAsync(result, cancellationToken), Times.Once);
        RefreshTokenRepositoryMock.Verify(r => r.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task ValidateRefreshTokenAsync_ShouldThrowInvalidRefreshTokenException_WhenTokenIsNullOrEmpty()
    {
        // Arrange
        var emptyToken = string.Empty;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRefreshTokenException>(() =>
            RefreshTokenService.ValidateRefreshTokenAsync(DefaultUser.Id, emptyToken, CancellationToken.None));
    }

    [Fact]
    public async Task RevokeRefreshTokenAsync_ShouldRemoveTokenAndReturnTrue()
    {
        // Arrange
        var validToken = new RefreshToken
        {
            UserId = DefaultUser.Id,
            Token = "validToken"
        };
        var cancellationToken = CancellationToken.None;

        RefreshTokenRepositoryMock
            .Setup(r => r.GetByUserIdAndRefreshTokenAsync(DefaultUser.Id, validToken.Token, cancellationToken))
            .ReturnsAsync(validToken);

        // Act
        var result = await RefreshTokenService.RevokeRefreshTokenAsync(DefaultUser.Id, validToken.Token, cancellationToken);

        // Assert
        result.Should().BeTrue();
        RefreshTokenRepositoryMock.Verify(r => r.RemoveTokenAsync(validToken), Times.Once);
        RefreshTokenRepositoryMock.Verify(r => r.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowInvalidRefreshTokenException_WhenTokenIsInvalid()
    {
        // Arrange
        var request = new RefreshTokenRequest("invalidToken", DefaultUser.Id);
        var cancellationToken = CancellationToken.None;

        RefreshTokenRepositoryMock
            .Setup(r => r.GetByUserIdAndRefreshTokenAsync(request.UserId, request.RefreshToken, cancellationToken))
            .ReturnsAsync((RefreshToken)null);

        // Act
        var action = async () => await RefreshTokenService.RefreshTokenAsync(request, cancellationToken);

        // Assert
        await action.Should().ThrowAsync<InvalidRefreshTokenException>()
            .WithMessage($"Invalid refresh token: {request.RefreshToken}");
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldReturnNewAccessToken_WhenTokenIsValid()
    {
        // Arrange
        var request = new RefreshTokenRequest("validToken", DefaultUser.Id);
        var validToken = new RefreshToken
        {
            UserId = DefaultUser.Id,
            Token = request.RefreshToken,
            ExpiryTime = DateTimeOffset.UtcNow.AddMinutes(10)
        };
        var roles = new List<string> { "User" };
        var newAccessToken = "newAccessToken";
        var cancellationToken = CancellationToken.None;

        RefreshTokenRepositoryMock
            .Setup(r => r.GetByUserIdAndRefreshTokenAsync(request.UserId, request.RefreshToken, cancellationToken))
            .ReturnsAsync(validToken);

        UserManagerMock.Setup(um => um.FindByIdAsync(request.UserId.ToString())).ReturnsAsync(DefaultUser);
        UserManagerMock.Setup(um => um.GetRolesAsync(DefaultUser)).ReturnsAsync(roles);
        AccessTokenServiceMock.Setup(a => a.GenerateAccessToken(DefaultUser, roles)).Returns(newAccessToken);

        // Act
        var result = await RefreshTokenService.RefreshTokenAsync(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be(newAccessToken);
        result.RefreshToken.Should().Be(request.RefreshToken);
    }
}
