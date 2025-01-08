using Application.Abstractions.DTOs;
using Application.Abstractions.Services.Email;
using Application.Options;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
namespace Application.Services.Auth;

public class ConfirmMessageSenderService(IEmailSenderService emailSenderService,
    IOptions<EmailSenderOptions> emailSenderOptions,
    UserManager<User> userManager
    ) : IConfirmMessageSenderService
{
    public async Task SendEmailConfirmMessageAsync(User user, CancellationToken cancellationToken)
    {
        if (emailSenderOptions is null)
        {
            throw new ArgumentNullException(nameof(emailSenderOptions));
        }
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmUrl = $"{emailSenderOptions.Value.ConfirmUrl}?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        var emailBody = GenerateEmailBody(user, confirmUrl);
        var message = GenerateEmailMessage(user, "Email confirmation", emailBody);
        await emailSenderService.SendEmailAsync(message, cancellationToken);
    }

    private static string GenerateEmailBody(User user, string confirmUrl)
    {
        return $"<h1>Dear {user.UserName}! Thank you for signing up.</h1>" +
            $"<p>Please confirm your email by clicking <a href='{System.Text.Encodings.Web.HtmlEncoder.Default.Encode(confirmUrl)}'>here</a>.</p>";
    }

    private static EmailMessage GenerateEmailMessage(User user, string subject, string emailBody)
    {
        return new EmailMessage(user.Email, subject, emailBody, user.UserName);
    }

}
