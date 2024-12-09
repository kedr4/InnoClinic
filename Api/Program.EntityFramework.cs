using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Presentation;

public static class ProgramEntityFramework
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        //// ��������� SQL Server
        //services.AddDbContext<DatabaseContext>(options =>
        //options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddDbContext<AuthDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

        return services;
    }
}