using Application.Abstractions.Persistance.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

public class RefreshTokenRepository(AuthDbContext context) : IRefreshTokenRepository
{
    public async Task CreateTokenAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        await context.RefreshTokens.AddAsync(token, cancellationToken);
    }

    public async Task<RefreshToken?> GetByUserIdAndRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        return await context.RefreshTokens
            .Include(token => token.User)
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Token == refreshToken, cancellationToken);
    }

    public async Task<RefreshToken?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await context.RefreshTokens
            .Where(token => token.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public void RemoveTokenAsync(RefreshToken token)
    {
        context.RefreshTokens.Remove(token);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}