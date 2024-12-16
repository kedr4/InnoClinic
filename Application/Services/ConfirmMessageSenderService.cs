using System.Text;
using Application.Abstractions.DTOs;
using Application.Abstractions.Services.Email;
using Application.Services.Email;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
namespace Application.Services.Auth;

public class ConfirmMessageSenderService(IEmailSenderService emailSenderService,
    IOptions<EmailSenderOptions> emailSenderOptions,
    UserManager<User> userManager) : IConfirmMessageSenderService
{
    public async Task SendEmailConfirmMessageAsync(User user, CancellationToken cancellationToken)
    {
        var token = await GenerateConfirmationToken(user);

        var confirmUrl = emailSenderOptions.Value.ConfirmUrl + $"{user.Id}/{token}";
        var emailBody = GenerateEmailBody(user, confirmUrl);
        var message = GenerateEmailMessage(user, "Email confirmation", emailBody);
        await emailSenderService.SendEmailAsync(message, cancellationToken);
    }

    private static string GenerateEmailBody(User user, string confirmUrl)
    {
        return $"<h1>Hello, {user.UserName}! Thank you for signing up.</h1>" +
            $"<p>Please confirm your email by clicking <a href='{System.Text.Encodings.Web.HtmlEncoder.Default.Encode(confirmUrl)}'>here</a>.</p>";
    }

    private static EmailMessage GenerateEmailMessage(User user, string subject, string emailBody)
    {
        return new EmailMessage(user.Email, subject, emailBody, user.UserName);
    }

    private async Task<string> GenerateConfirmationToken(User user)
    {
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var tokenGeneratedBytes = Encoding.UTF8.GetBytes(code);
        return WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
    }
}
