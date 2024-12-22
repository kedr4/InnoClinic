using Application.Abstractions.DTOs;
using Application.Abstractions.Services.Email;
using Application.Options;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Services.Email;

public class EmailSenderService(IOptions<EmailSenderOptions> emailSenderOptions, ISmtpClientService smtpClientService) : IEmailSenderService
{


    public async Task SendEmailAsync(EmailMessage mailMessage, CancellationToken cancellationToken)
    {
        var emailMessage = CreateEmailMessage(mailMessage);

        if (!smtpClientService.IsConnected)
        {
            await smtpClientService.ConnectAsync(cancellationToken);
        }

        await smtpClientService.SendAsync(emailMessage, cancellationToken);
    }

    private MimeMessage CreateEmailMessage(EmailMessage message)
    {
        var emailMessage = new MimeMessage();

        if (emailSenderOptions.Value.UserName is null)
        {
            throw new ArgumentNullException(nameof(emailSenderOptions));
        }

        emailMessage.From.Add(new MailboxAddress("InnoClinic", emailSenderOptions.Value.Sender));
        emailMessage.To.Add(new MailboxAddress(message.AddresseeName, message.Addressee));
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.Content
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();

        return emailMessage;
    }
}
