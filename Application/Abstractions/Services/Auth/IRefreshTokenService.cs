using Application.Abstractions.DTOs;
using Domain.Models;

namespace Application.Abstractions.Services.Auth;

public interface IRefreshTokenService
{
    public Task<RefreshToken> CreateUserRefreshTokenAsync(User user, CancellationToken cancellationToken);
    public Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken);
    public Task<bool> RevokeRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken);
    public Task<LoginUserResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
}
