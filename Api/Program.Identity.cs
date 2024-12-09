using Domain.Models;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;

namespace Presentation;

public static class ProgramIdentity

{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<UserManager<Doctor>>();
        services.AddScoped<UserManager<Patient>>();
        services.AddScoped<UserManager<Receptionist>>();

        return services;
    }
}