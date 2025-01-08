namespace Application.Abstractions.Services.Email;

public interface IConfirmMessageSenderService
{
    Task SendEmailConfirmMessageAsync(Domain.Models.User user, CancellationToken cancellationToken);
}