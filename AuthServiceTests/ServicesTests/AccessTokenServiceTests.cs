using Application.Services;
using AuthServiceTests.Fixtures;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthServiceTests.ServicesTests;

public class AccessTokenServiceTests : TestFixture
{
    private readonly AccessTokenService _accessTokenService;

    public AccessTokenServiceTests()
    {
        _accessTokenService = new AccessTokenService(MockJwtSettings.Object);
    }

    [Fact]
    public void GenerateAccessToken_ShouldReturnToken_WhenRolesAreProvided()
    {
        // Arrange
        var roles = new List<string> { "testRole1", "testRole2", "testRole3" };

        // Act
        var token = _accessTokenService.GenerateAccessToken(DefaultUser, roles);

        // Assert
        token.Should().NotBeNullOrEmpty();
    }


    [Fact]
    public void GenerateAccessToken_ShouldExpireCorrectly_WhenShortExpiryTimeIsSet()
    {
        // Arrange
        MockJwtSettings.Object.Value.ExpiryMinutes = 1;
        var roles = new List<string> { "testRole1" };
        var token = _accessTokenService.GenerateAccessToken(DefaultUser, roles);

        // Act
        Thread.Sleep(61000);
        var validationResult = ValidateToken(token);

        // Assert
        validationResult.Should().BeFalse();
    }

    private bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(MockJwtSettings.Object.Value.Secret);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = MockJwtSettings.Object.Value.Issuer,
                ValidateAudience = true,
                ValidAudience = MockJwtSettings.Object.Value.Audience,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
