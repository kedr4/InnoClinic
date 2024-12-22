using Application.Abstractions.DTOs;
using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services.Auth;
using Application.Exceptions;
using Application.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Application.Services;

public class RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IAccessTokenService accessTokenService, UserManager<User> userManager) : IRefreshTokenService
{

    public async Task<RefreshToken> CreateUserRefreshTokenAsync(User user, CancellationToken cancellationToken)
    {
        var refreshToken = GenerateRefreshToken(user);

        await refreshTokenRepository.CreateTokenAsync(refreshToken, cancellationToken);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));
        }

        ErrorCaster.CheckForUserIdEmptyException(userId);

        var refreshTokenEntity = await GetByUserIdAndRefreshTokenAsync(userId, refreshToken, cancellationToken);

        if (refreshTokenEntity is null || refreshTokenEntity.Token != refreshToken)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> RevokeRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await refreshTokenRepository.GetByUserIdAndRefreshTokenAsync(userId, refreshToken, cancellationToken);

        if (refreshToken is null)
        {
            return true;
        }

        if (refreshTokenEntity.UserId != userId)
        {
            return false;
        }

        refreshTokenRepository.RemoveTokenAsync(refreshTokenEntity);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static RefreshToken GenerateRefreshToken(User user)
    {
        ErrorCaster.CheckForUserIdEmptyException(user.Id);

        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = GenerateSecureRandomString(7),
            AddedTime = DateTimeOffset.UtcNow,
            ExpiryTime = DateTimeOffset.UtcNow.AddMonths(1),
            User = user
        };
    }

    private static string GenerateSecureRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var randomBytes = new byte[length];

        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        var builder = new StringBuilder(length);
        foreach (var b in randomBytes)
        {
            builder.Append(chars[b % chars.Length]);
        }

        return builder.ToString();
    }

    public async Task<LoginUserResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.userId.ToString());

        var result = await ValidateRefreshTokenAsync(request.userId, request.refreshToken, cancellationToken);

        if (!result)
        {
            throw new InvalidRefreshTokenException(request.refreshToken);
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = accessTokenService.GenerateAccessToken(user, roles);

        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidUserRoleException();
        }

        return new LoginUserResponse(user.Id, token, request.refreshToken);

    }

    private async Task<RefreshToken?> GetByUserIdAndRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {

        if (refreshToken is null)
        {
            throw new RefreshTokenIsNullException();
        }

        var refreshTokenEntity = await refreshTokenRepository.GetByUserIdAndRefreshTokenAsync(userId, refreshToken, cancellationToken);


        if (refreshTokenEntity is null)
        {
            throw new RefreshTokenIsNullException();
        }

        var currTime = DateTimeOffset.Now;

        if (refreshTokenEntity.ExpiryTime < currTime)
        {
            refreshTokenRepository.RemoveTokenAsync(refreshTokenEntity);

            return null;
        }

        return refreshTokenEntity;
    }

}
