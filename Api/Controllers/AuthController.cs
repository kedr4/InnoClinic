using Application.Abstractions.DTOs;
using Application.Abstractions.Services.Auth;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IRefreshTokenService _refreshTokenService;


    public AuthController(IAuthService authService, IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService)
    {
        _authService = authService;
        _accessTokenService = accessTokenService;
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("register-patient")]
    public async Task<IActionResult> RegisterPatientAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    { 
        var userId = await _authService.RegisterUserAsync(request.Email, request.Password, Roles.Patient, cancellationToken);

        return Ok(userId);
    }

    [HttpPost("register-doctor")]
    public async Task<IActionResult> RegisterDoctorAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userId = await _authService.RegisterUserAsync(request.Email, request.Password, Roles.Doctor, cancellationToken);

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
    public async Task<IActionResult> ConfirmMailAsync([FromQuery] Guid userId, string token, CancellationToken cancellationToken)
    {
        var request = new ConfirmMailRequest(userId, token);
        await _authService.ConfirmMailAsync(request, cancellationToken);

        return Ok(true);
    }

}
