using Application.Abstractions.Services.Auth;
using Application.Abstractions.Services.Email;
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

namespace Presentation;

public static class ApplicationInjection
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    
    public static IConfigurationBuilder AddApplicationUserSecrets(this IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());

        return configurationBuilder;
    }

    public static IServiceCollection AddApplicationOptions(this IServiceCollection services)
    {
        services.AddOptions<JwtSettingsOptions>()
            .BindConfiguration(nameof(JwtSettingsOptions));
        services.AddOptions<EmailSenderOptions>()
            .BindConfiguration(nameof(EmailSenderOptions));

        return services;

    }

    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = new JwtSettingsOptions();
        configuration.GetSection(nameof(JwtSettingsOptions)).Bind(jwtOptions);

        var key = jwtOptions.Secret;
        var issuer = jwtOptions.Issuer;
        var audience = jwtOptions.Audience;
        var expiryMinutes = jwtOptions.ExpiryMinutes;

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
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
                ClockSkew = TimeSpan.FromMinutes(expiryMinutes)
            };
        });

        return services;
    }


    public static IServiceCollection AddServices(this IServiceCollection services)
    {

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IConfirmMessageSenderService, ConfirmMessageSenderService>();
        services.AddScoped<ISmtpClientService, SmtpClientService>();

        return services;
    }

}
