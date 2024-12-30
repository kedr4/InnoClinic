using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services.Auth;
using Application.Abstractions.Services.Email;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthServiceTests;

public class TestFixture
{
    protected readonly Mock<UserManager<User>> UserManagerMock = new(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
    protected readonly Mock<IAccessTokenService> AccessTokenServiceMock = new();
    protected readonly Mock<IRefreshTokenService> RefreshTokenServiceMock = new();
    protected readonly Mock<IConfirmMessageSenderService> ConfirmMessageSenderServiceMock = new();
    protected readonly Mock<IRefreshTokenRepository> RefreshTokenRepositoryMock = new();

    public readonly Faker Faker = new();

    public readonly AuthService AuthService;
    public readonly RefreshTokenService RefreshTokenService;
    public readonly User DefaultUser;

    public TestFixture()
    {
        AuthService = new AuthService(
            UserManagerMock.Object,
            AccessTokenServiceMock.Object,
            RefreshTokenServiceMock.Object,
            ConfirmMessageSenderServiceMock.Object);

        RefreshTokenService = new RefreshTokenService(
            RefreshTokenRepositoryMock.Object,
            AccessTokenServiceMock.Object,
            UserManagerMock.Object);

        DefaultUser = new User
        {
            Id = Guid.NewGuid(),
            Email = Faker.Internet.Email(),
            UserName = Faker.Internet.UserName()
        };
    }
}
