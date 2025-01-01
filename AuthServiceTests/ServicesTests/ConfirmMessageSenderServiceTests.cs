using Application.Options;
using Application.Services.Auth;
using AuthServiceTests.Fixtures;


namespace AuthServiceTests.ServicesTests;

public class ConfirmMessageSenderServiceTests : TestFixture
{
    private readonly ConfirmMessageSenderService ConfirmMessageSenderService;

    public ConfirmMessageSenderServiceTests()
    {
        var emailSenderOptions = new EmailSenderOptions { ConfirmUrl = "https://localhost:44305/api/auth/confirm/" };
        MockEmailOptions
            .Setup(opt => opt.Value)
            .Returns(emailSenderOptions);

        ConfirmMessageSenderService = new ConfirmMessageSenderService(EmailSenderServiceMock.Object, MockEmailOptions.Object, UserManagerMock.Object);
    }

    [Fact]
    public async Task SendEmailConfirmMessageAsync_ShouldCallSendEmailAsync_WhenUserExists()
    {
        // Arrange
        var user = DefaultUser;

        UserManagerMock
            .Setup(um => um.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        UserManagerMock
            .Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
            .ReturnsAsync("token");

        // Act
        await ConfirmMessageSenderService.SendEmailConfirmMessageAsync(user, CancellationToken.None);

        // Assert
        EmailSenderServiceMock.Verify(service =>
            service.SendEmailAsync(It.Is<EmailMessage>(message =>
                message.Addressee == user.Email &&
                message.Subject == "Email confirmation" &&
                message.Content.Contains("https://localhost:44305/api/auth/confirm/")
            ), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}


