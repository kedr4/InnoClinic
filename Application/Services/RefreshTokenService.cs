using Application.Abstrsctions.Services;
using Domain.Models;

namespace Application.Services;
public class RefreshTokenService : IRefreshTokenService
{
    public RefreshToken GenerateRefreshToken(Guid userId)
    {
        throw new NotImplementedException();
    }

    public void RevokeRefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public bool ValidateRefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
