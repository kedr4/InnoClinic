using Business.Abstractions.Services;
using Business.Options;
using Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Business;

public static class BusinessInjection
{

    public static IServiceCollection AddBusiness(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IFilesService, FilesService>()
            .AddAuthenticationServices(configuration);

        return services;
    }


    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = new JwtSettingsOptions();
        configuration.GetSection(nameof(JwtSettingsOptions)).Bind(jwtOptions);

        var secret = jwtOptions.Secret;
        var issuer = jwtOptions.Issuer;
        var audience = jwtOptions.Audience;
        var expiryMinutes = jwtOptions.ExpiryMinutes;

        if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || expiryMinutes == 0)
        {
            throw new ArgumentException($"{secret} {issuer} {audience} {expiryMinutes} JWT settings are missing or incomplete.");
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });

        return services;
    }
}
