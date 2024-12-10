using Domain.Models;

namespace Application.Abstractions.Persistance.Repositories;

public interface IRefreshTokenRepository
{
    public Task CreateTokenAsync(RefreshToken token, CancellationToken cancellationToken);
    public Task<RefreshToken?> GetUserRefreshTokenAsync(Guid userId, CancellationToken cancellationToken);
    public Task RemoveTokenAsync(RefreshToken token);
    public Task SaveChangesAsync(CancellationToken cancellationToken);

}
