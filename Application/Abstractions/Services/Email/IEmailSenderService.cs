using Application.Abstractions.DTOs;
using System.Runtime.CompilerServices;

namespace Application.Abstractions.Services.Email;

public interface IEmailSenderService
{
    Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken);
}
