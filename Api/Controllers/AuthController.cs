﻿using Application.Abstractions.DTOs;
using Application.Abstractions.Services.Auth;
using Application.Filters;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ServiceFilter(typeof(ValidateModelFilter))]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IRefreshTokenService _refreshTokenService;


    public AuthController(IAuthService authService, IRefreshTokenService refreshTokenService)
    {
        _authService = authService;
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("register-patient")]
    public async Task<IActionResult> RegisterPatientAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userId = await _authService.RegisterUserAsync(request, Roles.Patient, cancellationToken);

        return Ok(userId);
    }

    [HttpPost("register-doctor")]
    public async Task<IActionResult> RegisterDoctorAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userId = await _authService.RegisterUserAsync(request, Roles.Doctor, cancellationToken);

        return Ok(userId);
    }


    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginUserAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync([FromBody] LogoutUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LogoutUserAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _refreshTokenService.RefreshTokenAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet("confirm-mail")]
    public async Task<IActionResult> ConfirmMailAsync([FromQuery] ConfirmMailRequest request, CancellationToken cancellationToken)
    {
        await _authService.ConfirmMailAsync(request, cancellationToken);

        return Ok(true);
    }
}
