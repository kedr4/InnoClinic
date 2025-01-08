using Application.Abstractions.DTOs;

namespace Application.Abstractions.Services.Email;

public interface IEmailSenderService
{
    Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken);
}
