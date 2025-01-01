﻿using Application.Abstractions.DTOs;
using Application.Abstractions.Services.Auth;
using Application.Abstractions.Services.Email;
using Application.Exceptions;
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

        if (!result.Succeeded)
        {
            throw new UserRegistrationException(result.Errors.ToString());
        }

        await userManager.AddToRoleAsync(user, role.ToString());
        await confirmMessageSenderService.SendEmailConfirmMessageAsync(user, cancellationToken);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description)); ;

            throw new UserRegistrationException(errors);
        }

        return user.Id;
    }


    public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest loginUserRequest, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(loginUserRequest.Email);

        if (user is null)
        {
            throw new UserNotFoundException(loginUserRequest.Email);
        }

        await CheckUserPasswordAsync(user, loginUserRequest.Password);

        //if (!user.EmailConfirmed)
        //{
        //    throw new EmailIsNotConfirmedException();
        //}

        var userRoles = await userManager.GetRolesAsync(user);
        var accessToken = accessTokenService.GenerateAccessToken(user, userRoles);

        var existingRefreshToken = await refreshTokenService.GetByUserIdAsync(user.Id, cancellationToken);

        if (existingRefreshToken is not null)
        {
            await refreshTokenService.RevokeRefreshTokenAsync(user.Id, existingRefreshToken.Token, cancellationToken);
        }

        var newRefreshToken = await refreshTokenService.CreateUserRefreshTokenAsync(user, cancellationToken);

        return new LoginUserResponse(user.Id, accessToken, newRefreshToken.Token);
    }

    public Task<bool> LogoutUserAsync(LogoutUserRequest request, CancellationToken cancellationToken)
    {
        return refreshTokenService.RevokeRefreshTokenAsync(request.UserId, request.RefreshToken, cancellationToken);
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

    private async Task CheckUserPasswordAsync(User user, string password)
    {

        bool isPasswordValid = await userManager.CheckPasswordAsync(user, password);

        if (!isPasswordValid)
        {
            throw new InvalidLoginException();
        }
    }
}
