using Domain.Models;

namespace Application.Abstrsctions.Services;
public interface IRefreshTokenService
{
    public RefreshToken GenerateRefreshToken(Guid userId);
    public bool ValidateRefreshToken(string refreshToken);
    public void RevokeRefreshToken(string refreshToken);
}
