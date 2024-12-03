using Application.Abstrsctions.Persistance.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AuthDbContext _context;

    public RefreshTokenRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Guid userId, RefreshToken refreshToken)
    {
        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshToken.Token,
            CreatedAt = refreshToken.CreatedAt,
            ExpiresAt = refreshToken.ExpiresAt,
            IsRevoked = refreshToken.IsRevoked
        };

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        return newRefreshToken.Id;
    }

    public async Task UpdateAsync(Guid userId, RefreshToken refreshToken)
    {
        var existingRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken.Token);

        if (existingRefreshToken != null)
        {
            existingRefreshToken.IsRevoked = refreshToken.IsRevoked;
            existingRefreshToken.ExpiresAt = refreshToken.ExpiresAt;

            await _context.SaveChangesAsync();
        }
    }

    public async Task<RefreshToken> GetByUserIdAndTokenAsync(Guid userId, string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken && !rt.IsRevoked);

        return token;
    }
}