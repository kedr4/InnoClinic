namespace Application.Abstractions.Services;

public interface IAccountService
{
    public Task<bool> IsAccountExistsAsync(Guid accountId, CancellationToken cancellationToken);
}
