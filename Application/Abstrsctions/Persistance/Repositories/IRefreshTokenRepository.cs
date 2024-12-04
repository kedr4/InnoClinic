using Domain.Models;

namespace Application.Abstrsctions.Persistance.Repositories;

public interface IRefreshTokenRepository
{

    public Task<Guid> AddAsync(RefreshToken refreshToken);
    public Task<RefreshToken> GetByUserIdAndTokenAsync(Guid userId, string refreshToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
    // AddAsync
    // UpdateAsync
    // GetByUSerIdAndToken(userId, refreshToken) !Revoked

}
