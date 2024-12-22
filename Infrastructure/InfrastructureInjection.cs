using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services.Auth;
using Domain.Models;
using Infrastructure.Options;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddIdentityServices(services);
        AddInfrastructureOptions(services);
        AddEntityFramework(services, configuration);
        AddRepositories(services);
        AddRoles(services);

        return services;
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<User, UserRole>()
                   .AddEntityFrameworkStores<AuthDbContext>()
                   .AddDefaultTokenProviders()
                   .AddRoles<UserRole>();

        return services;
    }

    private static IServiceCollection AddInfrastructureOptions(this IServiceCollection services)
    {
        services.AddOptions<DatabaseOptions>()
            .BindConfiguration(nameof(DatabaseOptions));

        return services;
    }

    private static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseOptions = new DatabaseOptions();
        configuration.GetSection(nameof(DatabaseOptions)).Bind(databaseOptions);
        var connectionString = databaseOptions.ConnectionString;

        if (connectionString is null)
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

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }

    private static IServiceCollection AddRoles(this IServiceCollection services)
    {
        services.AddScoped<IRolesService, RolesService>();

        return services;
    }
}
