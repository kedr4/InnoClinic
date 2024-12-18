using Microsoft.OpenApi.Models;
using Presentation.Middleware;
using Serilog;

namespace Presentation;

public static class PresentationInjection
{
    public static IApplicationBuilder UseCustomExceptionHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        return app;
    }

    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        return builder;
    }

    public static WebApplication UseSerilog(this WebApplication builder, IConfiguration configuration)
    {
        builder.UseSerilogRequestLogging();

        return builder;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
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

    public static IApplicationBuilder UseSwagger(this IApplicationBuilder applicationBuilder, WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI();
        }

        return applicationBuilder;
    }
}
