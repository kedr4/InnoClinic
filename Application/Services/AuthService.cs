using Application.Abstractions.DTOs;
using Application.Abstractions.Services;
using Application.Exceptions;
using Application.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthService
(
     UserManager<User> userManager,
     IAccessTokenService accessTokenService,
     IRefreshTokenService refreshTokenService
) : IAuthService
{

    public async Task<Guid> RegisterUserAsync(string email, string password, Roles role, CancellationToken cancellationToken)
    {
        if (await userManager.FindByEmailAsync(email) != null)
        {
            throw new Exception("User already exists");
        }

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = email.Split('@')[0]
        };

        var result = await userManager.CreateAsync(user, password);
        ErrorCaster.CheckForUserRegistrationException(result);

        return user.Id;
    }

    public async Task<bool> CheckUserPasswordAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            throw new UserNotFoundException(user.Id);
        }

        Tuple<bool, Guid> loginResult;
        bool checkLoginResult = false;

        checkLoginResult = await userManager.CheckPasswordAsync(user, password);
        loginResult = new Tuple<bool, Guid>(checkLoginResult, user.Id);
        ErrorCaster.CheckForInvalidLoginException(checkLoginResult);

        return checkLoginResult;
    }

    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest loginUserRequest, CancellationToken cancellationToken)
    {
        var isValid = await CheckUserPasswordAsync(loginUserRequest.Email, loginUserRequest.Password);

        if (!isValid)
        {
            throw new UnauthorizedAccessException("Password or email is incorrect");
        }

        var user = await userManager.FindByEmailAsync(loginUserRequest.Email);
        ErrorCaster.CheckForUserNotFoundException(user is null, loginUserRequest.Email);

        var userRoles = await accessTokenService.GetRolesAsync(user);
        var accessToken = accessTokenService.GenerateAccessToken(user, userRoles);

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new UnauthorizedAccessException("User role is not suitable");
        }

        var refreshToken = await refreshTokenService.CreateUserRefreshTokenAsync(user, cancellationToken);

        return new LoginUserResponse(user, accessToken, refreshToken.Token);
    }

    public async Task<bool> LogoutUserAsync(LogoutUserRequest request, CancellationToken cancellationToken)
    {
        var result = await refreshTokenService.RevokeRefreshTokenAsync(request.UserId, cancellationToken);

        return result;
    }
}
