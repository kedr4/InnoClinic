using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services;
using Application.Services;
using Domain.Models;
using Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Presentation;

public static class ProgramServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IPatientsRepository, PatientsRepository>();
        services.AddScoped<IDoctorsRepository, DoctorsRepository>();
        services.AddScoped<IReceptionistsRepository, ReceptionistsRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}