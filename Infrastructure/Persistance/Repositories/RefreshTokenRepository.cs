﻿using Application.Abstractions.Persistance.Repositories;
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

    public async Task CreateTokenAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        await context.RefreshTokens.AddAsync(token, cancellationToken);
    }

    public async Task<RefreshToken?> GetUserRefreshTokenAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await context.RefreshTokens
            .Include(token => token.User)
            .FirstOrDefaultAsync(x =>
                x.UserId == userId, cancellationToken);
    }

    public Task RemoveTokenAsync(RefreshToken token)
    {
        context.RefreshTokens.Remove(token);

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}