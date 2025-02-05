using Business.Repositories.Interfaces;
using DataAccess.Helpers;
using DataAccess.Options;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
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

    private static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidation<MongoDBSettings>();
        services.AddScoped<MongoDBClient>();
        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();

        return services;
    }
}
