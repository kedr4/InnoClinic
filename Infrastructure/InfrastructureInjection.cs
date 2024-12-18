using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services.Auth;
using Application.Abstractions.Services.Email;
using Application.Helpers;
using Application.Services;
using Application.Services.Auth;
using Application.Services.Email;
using Domain.Models;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<User, UserRole>()
                   .AddEntityFrameworkStores<AuthDbContext>()
                   .AddDefaultTokenProviders()
                   .AddRoles<UserRole>();

        return services;
    }

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

    public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        //in memory setup
        //    services.AddDbContext<AuthDbContext>(options =>
        //options.UseInMemoryDatabase("InMemoryDb"));

        services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddProgramOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettingsOptions>(configuration.GetSection("JwtSettingsOptions"));
        services.Configure<EmailSenderOptions>(configuration.GetSection("EmailSenderOptions"));

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IConfirmMessageSenderService, ConfirmMessageSenderService>();
        services.AddScoped<ISmtpClientService, SmtpClientService>();

        return services;
    }
}
