using Application.Abstractions.DTOs;
using Application.Abstractions.Services.Auth;
using Application.Abstractions.Services.Email;
using Application.Exceptions;
using Application.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthService
(
     UserManager<User> userManager,
     IAccessTokenService accessTokenService,
     IRefreshTokenService refreshTokenService,
     IConfirmMessageSenderService confirmMessageSenderService
) : IAuthService
{

    public async Task<Guid> RegisterUserAsync(string email, string password, Roles role, CancellationToken cancellationToken)
    {
        if (await userManager.FindByEmailAsync(email) is not null)
        {
            throw new UserAlreadyExistsException();
        }

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = email.Split('@')[0]
        };

        var result = await userManager.CreateAsync(user, password);
         
        //await userManager.AddToRoleAsync(user, role.ToString()); ???????????????????????????????????????????????????????????????????????????????????????????????????????

        await confirmMessageSenderService.SendEmailConfirmMessageAsync(user, cancellationToken);

        ErrorCaster.CheckForUserRegistrationException(result);

        return user.Id;
    }


    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest loginUserRequest, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(loginUserRequest.Email);
        var isValid = await CheckUserPasswordAsync(user, loginUserRequest.Email, loginUserRequest.Password);

        if (!isValid)
        {
            throw new UnauthorizedAccessException("Password or email is incorrect");
        }

        var userRoles = await accessTokenService.GetRolesAsync(user);
        var accessToken = accessTokenService.GenerateAccessToken(user, userRoles);

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new UnauthorizedAccessException("User role is not suitable");
        }

        //if (user.RefreshToken.Token is not null)
        //{
        //    refreshTokenService.RevokeRefreshTokenAsync(user.Id, cancellationToken);
        //}

        //var refreshTokenRequest = new RefreshTokenRequest(user.RefreshToken.Token, user.Id);

        //var refreshToken = await refreshTokenService.RefreshTokenAsync(refreshTokenRequest, cancellationToken);

        var refreshToken = await refreshTokenService.CreateUserRefreshTokenAsync(user, cancellationToken);

        return new LoginUserResponse(user.Id, accessToken, refreshToken.Token);
    }

    public async Task<bool> LogoutUserAsync(LogoutUserRequest request, CancellationToken cancellationToken)
    {
        var result = await refreshTokenService.RevokeRefreshTokenAsync(request.UserId, cancellationToken);

        return result;
    }

    private async Task<bool> CheckUserPasswordAsync(User user, string email, string password)
    {
        if (user is null)
        {
            throw new UserIsNullException();
        }

        bool isPasswordValid = await userManager.CheckPasswordAsync(user, password);
        ErrorCaster.CheckForInvalidLoginException(isPasswordValid);

        return isPasswordValid;
    }
}
