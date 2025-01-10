using Business.Repositories.Interfaces;
using DataAccess.Helpers;
using DataAccess.Options;
using DataAccess.Repository;
using DataAccess.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class DataAccessInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongo(configuration);

        return services;
    }

    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidation<MongoDBSettings>();
        services.AddSingleton<MongoDBClient>();
        services.AddScoped<IOfficeRepository, OfficeRepository>();

        return services;
    }
}
