using Application.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Presentation;

public static class ProgramAuthentication
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettingsOptions>>();
        var key = options.Value.Secret;
        var issuer = options.Value.Issuer;
        var audience = options.Value.Audience;
        var expiryMinutes = options.Value.ExpiryMinutes;

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
        {
            throw new ArgumentException("JWT settings are missing or incomplete.");
        }

        services.AddIdentityServices();


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });

        return services;
    }
}