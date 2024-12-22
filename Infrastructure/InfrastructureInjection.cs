using Application.Abstractions.Persistance.Repositories;
using Application.Options;
using Domain.Models;
using Infrastructure.Options;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

    public static IConfigurationBuilder AddInfrastructureUserSecrets(this IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());

        return configurationBuilder;
    }

    public static IServiceCollection AddInfrastructureOptions(this IServiceCollection services)
    {
        services.AddOptions<DatabaseOptions>()
            .BindConfiguration(nameof(DatabaseOptions));

        return services;
    }

    public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        //in memory setup
        //    services.AddDbContext<AuthDbContext>(options =>
        //options.UseInMemoryDatabase("InMemoryDb"));

        var databaseOptions = new DatabaseOptions();
        configuration.GetSection(nameof(DatabaseOptions)).Bind(databaseOptions);

        var connectionString = databaseOptions.ConnectionString;

        if (connectionString is null)
        {
            throw new ArgumentNullException("Connection string is null");
        }

        services.AddDbContext<AuthDbContext>(options =>
          options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }
}
