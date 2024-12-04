﻿using Application.Abstrsctions.Persistance.Repositories;
using Application.Abstrsctions.Services;
using Application.Helpers;
using Domain.Models;
using System.Security.Cryptography;

namespace Application.Services;

public class RefreshTokenService(IRefreshTokenRepository refreshTokenRepository) : IRefreshTokenService
{

    public RefreshToken GenerateRefreshToken(Guid userId)
    {
        ErrorCaster.CheckForUserIdEmptyException(userId);

        var refreshToken = new RefreshToken()
        {

            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),
            IsRevoked = false,
            UserId = userId,
        };

        return refreshToken;
    }

    public async Task SetRefreshToken(Guid userId, RefreshToken refreshToken,CancellationToken  cancellationToken)
    {
        if (refreshToken is null)
        {
            throw new ArgumentNullException(nameof(refreshToken));
        }

        ErrorCaster.CheckForUserIdEmptyException(userId);

        await refreshTokenRepository.AddAsync(refreshToken);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);
        //AddAsync
    }

    public async Task RevokeRefreshToken(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));
        }

        ErrorCaster.CheckForUserIdEmptyException(userId);

        var refreshTokenEntity = await refreshTokenRepository.GetByUserIdAndTokenAsync(userId, refreshToken);
        ErrorCaster.CheckForRefreshTokenNotFoundException(refreshTokenEntity);

        refreshTokenEntity.IsRevoked = true;

        await refreshTokenRepository.SaveChangesAsync(cancellationToken);      
        // тут устанавливать isRevoked = true
    }

    public async Task<bool> ValidateRefreshToken(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));
        }

        ErrorCaster.CheckForUserIdEmptyException(userId);

        var refreshTokenEntity = await refreshTokenRepository.GetByUserIdAndTokenAsync(userId, refreshToken);

        if (refreshToken is null || refreshTokenEntity.IsRevoked)
        {
            return false;
        }

        refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return true;
        // тут updateAsync и проверять по isRevoked
    }
}
