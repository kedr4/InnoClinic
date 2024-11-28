using Application.Abstrsctions.Services;
using Domain.Models;

namespace Application.Services;
public class RefreshTokenService : IRefreshTokenService
{
    public Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task RevokeRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
