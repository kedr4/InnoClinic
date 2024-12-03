using Domain.Models;

namespace Application.Abstrsctions.Services;
public interface IRefreshTokenService
{
    public Task SetRefreshToken(Guid userId, RefreshToken refreshToken);
    public RefreshToken GenerateRefreshToken(Guid userId);
    public Task<bool> ValidateRefreshToken(Guid userId, string refreshToken);
    public Task RevokeRefreshToken(Guid userId, string refreshToken);
}
