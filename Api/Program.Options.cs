using Application.Abstractions.Services.Email;
using Application.Helpers;

namespace Presentation;

public static class ProgramOptions
{
    public static IServiceCollection AddProgramOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettingsOptions>(configuration.GetSection("JwtSettingsOptions"));
        services.Configure<EmailSenderOptions>(configuration.GetSection("EmailSenderOptions"));

        return services;
    }
}