using Application.Abstractions.Services.Auth;
using Application.Abstractions.Services.Email;
using Application.Helpers;
using Application.Options;
using Application.Services;
using Application.Services.Auth;
using Application.Services.Email;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Application;

public static class ApplicationInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddOptionsWithValidation<JwtSettingsOptions>()
            .AddOptionsWithValidation<EmailSenderOptions>()
            .AddAuthenticationServices(configuration)
            .AddAuthorization()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = new JwtSettingsOptions();
        configuration.GetSection(nameof(JwtSettingsOptions)).Bind(jwtOptions);

        var key = jwtOptions.Secret;
        var issuer = jwtOptions.Issuer;
        var audience = jwtOptions.Audience;
        var expiryMinutes = jwtOptions.ExpiryMinutes;

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || expiryMinutes == 0)
        {
            throw new ArgumentException("JWT settings are missing or incomplete.");
        }

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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IRefreshTokenService, RefreshTokenService>()
            .AddScoped<IAccessTokenService, AccessTokenService>()
            .AddScoped<IEmailSenderService, EmailSenderService>()
            .AddScoped<IConfirmMessageSenderService, ConfirmMessageSenderService>()
            .AddScoped<ISmtpClientService, SmtpClientService>();

        return services;
    }
}
