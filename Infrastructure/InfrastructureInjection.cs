using Application.Abstractions.Persistance.Repositories;
using Application.Helpers;
using Domain.Models;
using Infrastructure.Options;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentityServices()
            .AddOptionsWithValidation<DatabaseOptions>()
            .AddEntityFramework(configuration)
            .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<User, UserRole>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
        })
                   .AddEntityFrameworkStores<AuthDbContext>()
                   .AddDefaultTokenProviders()
                   .AddRoles<UserRole>();

        return services;
    }

    private static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseOptions = new DatabaseOptions();
        configuration.GetSection(nameof(DatabaseOptions)).Bind(databaseOptions);
        var connectionString = databaseOptions.ConnectionString;

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException("Connection string is null");
        }

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        services.AddDbContext<AuthDbContext>(options =>
                {
                    switch (environment)
                    {
                        case "Development":
                            {
                                options.UseInMemoryDatabase("InMemoryDb");

                                break;
                            }
                        case "Production":
                            {
                                options.UseSqlServer(databaseOptions.ConnectionString);

                                break;
                            }
                        default:
                            {
                                throw new Exception("Environment is not set");
                            }
                    }
                });

        return services;
    }
}
