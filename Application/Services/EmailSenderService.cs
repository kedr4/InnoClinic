using Application.Abstractions.DTOs;
using Application.Abstractions.Services.Email;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Services.Email;

public class EmailSenderService(IOptions<EmailSenderOptions> options, ISmtpClientService _smtpClientService) : IEmailSenderService
{
    private readonly EmailSenderOptions _options = options.Value;

    public Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        return SendAsync(CreateEmailMessage(message), cancellationToken);
    }

    private MimeMessage CreateEmailMessage(EmailMessage message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("InnoClinic", _options.Sender));
        emailMessage.To.Add(new MailboxAddress(message.AddresseeName, message.Addressee));
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.Content
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();

        return emailMessage;
    }

    private async Task SendAsync(MimeMessage mailMessage, CancellationToken cancellationToken)
    {
        if (!_smtpClientService.IsConnected)
        {
            await _smtpClientService.ConnectAsync(cancellationToken);
        }

        await _smtpClientService.SendAsync(mailMessage, cancellationToken);
    }
}
