using Domain.Models;

namespace Application.Abstrsctions.Services;
public interface IRefreshTokenService
{
    public Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId);
    public Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    public Task RevokeRefreshTokenAsync(string refreshToken);
}
