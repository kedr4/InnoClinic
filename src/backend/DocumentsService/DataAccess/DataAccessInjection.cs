using DataAccess.Abstractions.Repositories;
using DataAccess.Abstractions.Services;
using DataAccess.Helpers;
using DataAccess.Options;
using DataAccess.Repositories;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class DataAccessInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<DocumentsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                opt => opt.EnableRetryOnFailure()))
            .AddOptionsWithValidation<BlobOptions>()
            .AddScoped<IBlobStorageRepository, BlobStorageRepository>()
            .AddScoped<IFileDataRepository, FileDataRepository>();

        return services;
    }
}
