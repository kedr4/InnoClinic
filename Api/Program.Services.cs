using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services;
using Application.Services;
using Infrastructure.Persistance.Repositories;

namespace Presentation;

public static class ProgramServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();

        return services;
    }
}