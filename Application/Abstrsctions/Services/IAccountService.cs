namespace Application.Abstrsctions.Services;

public interface IAccountService
{
    public Task<bool> IsAccountExistsAsync(Guid accountId, CancellationToken cancellationToken);
}
