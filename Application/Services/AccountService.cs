using Application.Abstractions.Services;

namespace Application.Services;

public class AccountService : IAccountService
{
    public Task<bool> IsAccountExistsAsync(Guid accountId, CancellationToken cancellationToken) => Task.FromResult(true);
}
