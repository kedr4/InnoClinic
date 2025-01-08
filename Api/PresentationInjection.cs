using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Presentation;

public static class PresentationInjection
{
    public static WebApplicationBuilder AddPresentationServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddSwagger();
        builder.AddApplicationSerilog();
        builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

        return builder;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Введите 'Bearer' [пробел] и ваш токен в поле ниже. \r\n\r\nПример: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
        });

        return services;
    }

    private static WebApplicationBuilder AddApplicationSerilog(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        return builder;
    }
}
