using Application.Abstractions.DTOs;
using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services.Auth;
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

        var refreshTokenEntity = await GetUserRefreshTokenAsync(userId, cancellationToken);


        if (refreshTokenEntity is null || refreshTokenEntity.Token != refreshToken)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> RevokeRefreshTokenAsync(Guid userId, CancellationToken cancellationToken)
    {
        var refreshToken = await GetUserRefreshTokenAsync(userId, cancellationToken);

        if (refreshToken is null)
        {
            return true;
        }

        if (refreshToken.UserId != userId)
        {
            return false;
        }

        await refreshTokenRepository.RemoveTokenAsync(refreshToken);
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

    private async Task<RefreshToken?> GetUserRefreshTokenAsync(Guid userId, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetUserRefreshTokenAsync(userId, cancellationToken);

        var currTime = DateTimeOffset.Now;

        if (refreshToken.ExpiryTime < currTime)
        {
            await refreshTokenRepository.RemoveTokenAsync(refreshToken);

            return null;
        }

        return refreshToken;
    }

    public async Task<LoginUserResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.userId.ToString());

        var result = await ValidateRefreshTokenAsync(request.userId, request.refreshToken, cancellationToken);

        if (!result)
        {
            throw new InvalidOperationException("Refresh token is not valid");
        }

        var roles = await accessTokenService.GetRolesAsync(user);
        var token = accessTokenService.GenerateAccessToken(user, roles);

        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("User role is not suitable");
        }

        return new LoginUserResponse(user.Id, token, request.refreshToken);

    }
}
