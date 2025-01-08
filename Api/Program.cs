using Application;
using Infrastructure;
using Presentation.Helpers;
using Serilog;

namespace Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        IConfiguration configuration = builder.Configuration;
        var environment = builder.Environment;

        builder.Configuration
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appettings.{environment}.json", true)
            .AddEnvironmentVariables()
            .Build();

        builder.Services.AddInfrastructureServices(configuration);
        builder.Services.AddApplicationServices(configuration);
        builder.Services.AddControllers();
        builder.AddPresentationServices(configuration);

        builder.Services.AddOpenApi();

        var app = builder.Build();

        await app.SeedDatabaseAsync();

        app.ConfigureMiddlewares();
        app.MapControllers();

        app.Run();
    }
}
