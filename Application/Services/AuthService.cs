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

        if (!user.EmailConfirmed)
        {
            throw new EmailIsNotConfirmedException();
        }

        var userRoles = await accessTokenService.GetRolesAsync(user);
        var accessToken = accessTokenService.GenerateAccessToken(user, userRoles);

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new UnauthorizedAccessException("User role is not suitable");
        }


        var existingToken = await refreshTokenRepository.GetUserRefreshTokenAsync(user.Id, cancellationToken);

        if (existingToken is not null)
        {
            if (existingToken.ExpiryTime < DateTime.UtcNow)
            {
                await refreshTokenService.RevokeRefreshTokenAsync(user.Id, cancellationToken);
                existingToken = await refreshTokenService.CreateUserRefreshTokenAsync(user, cancellationToken);
            }
        }
        else
        {
            existingToken = await refreshTokenService.CreateUserRefreshTokenAsync(user, cancellationToken);
        }

        return new LoginUserResponse(user.Id, accessToken, existingToken.Token);
    }

    public async Task<bool> LogoutUserAsync(LogoutUserRequest request, CancellationToken cancellationToken)
    {
        var result = await refreshTokenService.RevokeRefreshTokenAsync(request.UserId, cancellationToken);

        return result;
    }

    public async Task ConfirmMailAsync(ConfirmMailRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Code);

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
