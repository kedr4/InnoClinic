using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Presentation;

public static class ProgramEntityFramework
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        // in memory setup
    //    services.AddDbContext<AuthDbContext>(options =>
    //options.UseInMemoryDatabase("InMemoryDb"));

        services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}