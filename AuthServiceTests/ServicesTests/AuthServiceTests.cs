using Application.Exceptions;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthServiceTests.ServicesTests;

public class AuthServiceTests : TestFixture
{
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = AuthService;
    }

    [Fact]
    public async Task RegisterUserAsync_UserAlreadyExists_ThrowsException()
    {
        // Arrange
        var request = new CreateUserRequest(DefaultUser.Email, Faker.Internet.Password());
        UserManagerMock.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync(DefaultUser);

        // Act & Assert
        await _authService.Invoking(auth => auth.RegisterUserAsync(request, Roles.Patient, It.IsAny<CancellationToken>()))
            .Should().ThrowAsync<UserAlreadyExistsException>();
    }

    [Fact]
    public async Task RegisterUserAsync_ValidUser_RegistersSuccessfully()
    {
        // Arrange
        var request = new CreateUserRequest(Faker.Internet.Email(), Faker.Internet.Password());

        UserManagerMock.Setup(um => um.FindByEmailAsync(request.Email)).ReturnsAsync((User)null);
        UserManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), request.Password))
            .Callback<User, string>((createdUser, password) =>
            {
                createdUser.Id = DefaultUser.Id;
            })
            .ReturnsAsync(IdentityResult.Success);

        UserManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), Roles.Patient.ToString()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.RegisterUserAsync(request, Roles.Patient, It.IsAny<CancellationToken>());

        // Assert
        result.Should().Be(DefaultUser.Id);
        UserManagerMock.Verify(um => um.CreateAsync(It.Is<User>(u => u.Email == request.Email), request.Password), Times.Once);
        ConfirmMessageSenderServiceMock.Verify(cs => cs.SendEmailConfirmMessageAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginUserAsync_UserNotFound_ThrowsException()
    {
        // Arrange
        var loginRequest = new LoginUserRequest(Faker.Internet.Email(), Faker.Internet.Password());
        UserManagerMock.Setup(um => um.FindByEmailAsync(loginRequest.Email)).ReturnsAsync((User)null);

        // Act & Assert
        await _authService.Invoking(auth => auth.LoginUserAsync(loginRequest, It.IsAny<CancellationToken>()))
            .Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task LoginUserAsync_ValidCredentials_ReturnsTokens()
    {
        // Arrange
        var loginRequest = new LoginUserRequest(DefaultUser.Email, Faker.Internet.Password());
        var roles = new List<string> { "User" };
        var accessToken = Faker.Random.String(20);
        var refreshToken = Faker.Random.String(20);

        UserManagerMock.Setup(um => um.FindByEmailAsync(DefaultUser.Email)).ReturnsAsync(DefaultUser);
        UserManagerMock.Setup(um => um.CheckPasswordAsync(DefaultUser, loginRequest.Password)).ReturnsAsync(true);
        UserManagerMock.Setup(um => um.GetRolesAsync(DefaultUser)).ReturnsAsync(roles);
        AccessTokenServiceMock.Setup(at => at.GenerateAccessToken(DefaultUser, roles)).Returns(accessToken);
        RefreshTokenServiceMock.Setup(rt => rt.GetByUserIdAsync(DefaultUser.Id, It.IsAny<CancellationToken>())).ReturnsAsync((RefreshToken)null);
        RefreshTokenServiceMock.Setup(rt => rt.CreateUserRefreshTokenAsync(DefaultUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RefreshToken { Token = refreshToken });

        // Act
        var result = await _authService.LoginUserAsync(loginRequest, It.IsAny<CancellationToken>());

        // Assert
        result.UserId.Should().Be(DefaultUser.Id);
        result.AccessToken.Should().Be(accessToken);
        result.RefreshToken.Should().Be(refreshToken);
    }
}
