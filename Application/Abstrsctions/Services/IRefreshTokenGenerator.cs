namespace Application.Abstrsctions.Services;
public interface IRefreshTokenGenerator
{
    public Task<string> GenerateRefreshTokenAsync(Guid userId);
    public Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    public Task<string> RefreshAccessTokenAsync(string refreshToken);
    public Task RevokeRefreshTokenAsync(string refreshToken);
}
