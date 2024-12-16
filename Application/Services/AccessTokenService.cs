using Application.Abstractions.Services.Auth;
using Application.Helpers;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public class AccessTokenService : IAccessTokenService
{
    private readonly JwtSettingsOptions _jwtSettings;
    private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

    public AccessTokenService(IOptions<JwtSettingsOptions> jwtSettings, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
    }

    public string GenerateAccessToken(User user, IList<string> userRoles)
    {
        ErrorCaster.CheckForUserIdEmptyException(user.Id);

        var secretKey = _jwtSettings.Secret;
        var issuer = _jwtSettings.Issuer;
        var audience = _jwtSettings.Audience;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        claims.AddRange(userRoles.Select(role => new Claim("role:", role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public Task<IList<string>> GetRolesAsync(User user)
    {
        return _userManager.GetRolesAsync(user);
    }

}
