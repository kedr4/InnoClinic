using Domain.Models;

namespace Application.Abstrsctions.Persistance.Repositories;
public interface IRefreshTokenRepository
{
    public interface IRefreshTokenRepository
    {
        public Task<RefreshToken> AddAsync(RefreshToken refreshToken);
        public Task<RefreshToken> GetByTokenAsync(string token);
        public Task<bool> RevokeAsync(string token);
        public Task<bool> IsValidAsync(string token);
    }

}
