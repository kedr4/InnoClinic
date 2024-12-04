using Application.Abstrsctions.Services;
using Application.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register/patient")]
    public async Task<IActionResult> RegisterPatientAsync([FromBody] CreatePatientRequest request)
    {
        try
        {
            var userId = await _authService.RegisterPatientAsync(request);
            return CreatedAtAction(nameof(RegisterPatientAsync), new { userId });
        }
        catch (Exception ex)
        {
            // Log
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("register/doctor")]
    public async Task<IActionResult> RegisterDoctorAsync([FromBody] CreateDoctorRequest request)
    {
        try
        {
            var userId = await _authService.RegisterDoctorAsync(request);
            return CreatedAtAction(nameof(RegisterDoctorAsync), new { userId });
        }
        catch (Exception ex)
        {
            // Log
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("register/receptionist")]
    public async Task<IActionResult> RegisterReceptionistAsync([FromBody] CreateReceptionistRequest request)
    {
        var userId = await _authService.RegisterReceptionistAsync(request);

        return CreatedAtAction(nameof(RegisterReceptionistAsync), new { userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] Application.DTOs.Requests.LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequest request)
    {

        await _authService.LogoutAsync(request);

        return NoContent();
    }

    [HttpPost("delete-profile")]
    public async Task<IActionResult> DeleteProfileAsync([FromBody] DeleteProfileRequest request)
    {
        await _authService.DeleteProfileAsync(request);

        return NoContent();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        var newToken = await _authService.RefreshAccessTokenAsync(request);

        return Ok(newToken);
    }
}
