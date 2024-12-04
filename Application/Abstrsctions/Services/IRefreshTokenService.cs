using Domain.Models;

namespace Application.Abstrsctions.Services;

public interface IRefreshTokenService
{
    public Task SetRefreshToken(Guid userId, RefreshToken refreshToken, CancellationToken cancellationToken);
    public RefreshToken GenerateRefreshToken(Guid userId);
    public Task<bool> ValidateRefreshToken(Guid userId, string refreshToken, CancellationToken cancellationToken);
    public Task RevokeRefreshToken(Guid userId, string refreshToken, CancellationToken cancellationToken);
}
