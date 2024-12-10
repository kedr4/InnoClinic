using Domain.Models;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;

namespace Presentation;

public static class ProgramIdentity

{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<User, UserRole>()
                   .AddEntityFrameworkStores<AuthDbContext>()
                   .AddDefaultTokenProviders()
                   .AddRoles<UserRole>();

        return services;
    }
}