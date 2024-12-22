using Application.Abstractions.DTOs;
using Application.Abstractions.Persistance.Repositories;
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
     IRefreshTokenRepository refreshTokenRepository,
     IConfirmMessageSenderService confirmMessageSenderService
) : IAuthService
{

    public async Task<Guid> RegisterUserAsync(CreateUserRequest request, Roles role, CancellationToken cancellationToken)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not null)
        {
            throw new UserAlreadyExistsException();
        }

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.Email.Split('@')[0]
        };

        var result = await userManager.CreateAsync(user, request.Password);

        //await userManager.AddToRoleAsync(user, role.ToString()); //???????????????????????????????????????????????????????????????????????????????????????????????????????

        await confirmMessageSenderService.SendEmailConfirmMessageAsync(user, cancellationToken);

        ErrorCaster.CheckForUserRegistrationException(result);

        return user.Id;
    }


    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest loginUserRequest, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(loginUserRequest.Email);

        if (user == null)
        {
            throw new UserNotFoundException(loginUserRequest.Email);
        }

        var isValid = await CheckUserPasswordAsync(user, loginUserRequest.Email, loginUserRequest.Password);

        if (!isValid)
        {
            throw new UnauthorizedAccessException("Password or email is incorrect");
        }

        //if (!user.EmailConfirmed)
        //{
        //    throw new EmailIsNotConfirmedException();
        //}

        var userRoles = await userManager.GetRolesAsync(user);
        var accessToken = accessTokenService.GenerateAccessToken(user, userRoles);

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new UnauthorizedAccessException("User role is not suitable");
        }

        RefreshToken refreshToken = await refreshTokenRepository.GetByUserIdAndRefreshTokenAsync(user.Id, user.RefreshToken.Token, cancellationToken);
        if (refreshToken == null || refreshToken.ExpiryTime>=DateTimeOffset.Now)
        {
            refreshToken = await refreshTokenService.CreateUserRefreshTokenAsync(user, cancellationToken);
        }
        //if (user.RefreshToken is null)
        //{
        //    refreshToken = await refreshTokenService.CreateUserRefreshTokenAsync(user, cancellationToken);

        //}
        //else
        //{
        //    refreshToken = await refreshTokenRepository.GetByUserIdAndRefreshTokenAsync(user.Id, user.RefreshToken.Token, cancellationToken);
        //}

        return new LoginUserResponse(user.Id, accessToken, refreshToken.Token);
    }

    public async Task<bool> LogoutUserAsync(LogoutUserRequest request, CancellationToken cancellationToken)
    {
        var result = await refreshTokenService.RevokeRefreshTokenAsync(request.UserId, request.RefreshToken, cancellationToken);

        return result;
    }

    public async Task ConfirmMailAsync(ConfirmMailRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Token);

        if (!result.Succeeded)
        {
            throw new InvalidConfirmTokenException();

        }
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
