using Application.Abstrsctions.Services;

namespace Infrastructure.Services;
public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<string> RefreshRefreshTokenAsync(string refreshToken)
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
