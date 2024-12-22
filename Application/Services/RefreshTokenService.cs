using Application.Abstractions.DTOs;
using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services.Auth;
using Application.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
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

        if (userId == Guid.Empty)
        {
            throw new UserIdEmptyException();
        }

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

        if (refreshTokenEntity is null)
        {
            return false;
        }

        if (refreshTokenEntity.UserId != userId)
        {
            return false;
        }

        refreshTokenRepository.RemoveTokenAsync(refreshTokenEntity);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return true;
    }



    public async Task<LoginUserResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        var result = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken, cancellationToken);

        if (!result)
        {
            throw new InvalidRefreshTokenException(request.RefreshToken);
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = accessTokenService.GenerateAccessToken(user, roles);

        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidUserRoleException();
        }

        return new LoginUserResponse(user.Id, token, request.RefreshToken);

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

    private static RefreshToken GenerateRefreshToken(User user)
    {
        if (user.Id == Guid.Empty)
        {
            throw new UserIdEmptyException();
        }

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
        using var randomGenerator = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        randomGenerator.GetBytes(bytes);

        return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
    }

}
