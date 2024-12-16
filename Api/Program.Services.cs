using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services.Auth;
using Application.Abstractions.Services.Email;
using Application.Services;
using Application.Services.Auth;
using Application.Services.Email;
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
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IConfirmMessageSenderService, ConfirmMessageSenderService>();
        services.AddScoped<ISmtpClientService, SmtpClientService>();

        return services;
    }
}