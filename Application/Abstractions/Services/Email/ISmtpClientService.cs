using MailKit.Net.Smtp;
using MimeKit;

namespace Application.Abstractions.Services.Email;

public interface ISmtpClientService : ISmtpClient
{
    Task ConnectAsync(CancellationToken cancellationToken);
}
