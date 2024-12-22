using Domain.Models;

namespace Application.Abstractions.Persistance.Repositories;

public interface IRefreshTokenRepository
{
    public Task CreateTokenAsync(RefreshToken token, CancellationToken cancellationToken);
    public Task<RefreshToken?> GetByUserIdAndRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken);
    public void RemoveTokenAsync(RefreshToken token);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
