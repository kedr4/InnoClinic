using Application.Abstractions.Services.Email;
using Application.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace Application.Services.Email;

public class SmtpClientService(IOptions<EmailSenderOptions> options) : SmtpClient, ISmtpClientService
{
    public async Task ConnectAsync(CancellationToken cancellationToken)
    {
        await ConnectAsync(
            options.Value.SmtpServer,
            options.Value.Port,
            options.Value.UseSsl,
            cancellationToken
        );

        AuthenticationMechanisms.Remove("XOAUTH2");

        await AuthenticateAsync(
            options.Value.UserName,
            options.Value.Password,
            cancellationToken
        );
    }
}
