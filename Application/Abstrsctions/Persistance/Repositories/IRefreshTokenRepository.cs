using Domain.Models;

namespace Application.Abstrsctions.Persistance.Repositories;
public interface IRefreshTokenRepository
{

    public Task<Guid> AddAsync(Guid userId, RefreshToken refreshToken);
    public Task UpdateAsync(Guid userId, RefreshToken refreshToken);
    public Task<RefreshToken> GetByUserIdAndTokenAsync(Guid userId, string refreshToken);
    // AddAsync
    // UpdateAsync
    // GetByUSerIdAndToken(userId, refreshToken) !Revoked

}
