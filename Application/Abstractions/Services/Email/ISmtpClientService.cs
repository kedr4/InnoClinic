using MailKit.Net.Smtp;

namespace Application.Abstractions.Services.Email;

public interface ISmtpClientService : ISmtpClient
{
    Task ConnectAsync(CancellationToken cancellationToken);
}
