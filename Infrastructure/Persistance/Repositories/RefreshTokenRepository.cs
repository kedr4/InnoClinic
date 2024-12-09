using Application.Abstractions.Persistance.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

public class RefreshTokenRepository(AuthDbContext context) : IRefreshTokenRepository
{
    public async Task<Guid> AddAsync(RefreshToken refreshToken)
    {
        context.RefreshTokens.Add(refreshToken);
        return refreshToken.Id;
    }


    public async Task<RefreshToken> GetByUserIdAndTokenAsync(Guid userId, string refreshToken)
    {
        var token = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken && !rt.IsRevoked);

        return token;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync();
    }
}