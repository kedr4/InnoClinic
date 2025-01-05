using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services.Auth;
using Application.Abstractions.Services.Email;
using Application.Options;
using Application.Services;
using Microsoft.Extensions.Options;

namespace AuthServiceTests.Fixtures;

public class TestFixture
{
    protected readonly Mock<UserManager<User>> UserManagerMock = new(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
    protected readonly Mock<IAccessTokenService> AccessTokenServiceMock = new();
    protected readonly Mock<IRefreshTokenService> RefreshTokenServiceMock = new();
    protected readonly Mock<IConfirmMessageSenderService> ConfirmMessageSenderServiceMock = new();
    protected readonly Mock<IRefreshTokenRepository> RefreshTokenRepositoryMock = new();
    protected readonly Mock<IEmailSenderService> EmailSenderServiceMock = new();
    protected readonly Mock<IOptions<EmailSenderOptions>> MockEmailOptions = new();
    protected readonly Mock<IOptions<JwtSettingsOptions>> MockJwtSettings;
    protected readonly Faker Faker = new();
    protected readonly AuthService AuthService;
    protected readonly RefreshTokenService RefreshTokenService;
    protected readonly User DefaultUser;
    protected readonly List<string> DefaultRoles;

    public TestFixture()
    {
        MockJwtSettings = new Mock<IOptions<JwtSettingsOptions>>();

        MockJwtSettings.Setup(o => o.Value).Returns(new JwtSettingsOptions
        {
            Secret = "secretandsecureandsafeandlooo00ngkey!",
            Issuer = "SuperIssuer",
            Audience = "SuperAudience",
            ExpiryMinutes = 60
        });

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

        DefaultRoles = new List<string> { "TestRole1", "TestRole2", "TestRole3" };
        // вынести то, что не глобальное на всех
        // сделать базовцую фикстуру и от нее унаследовать конкретные под каждый сервис 
        // 
    }
}