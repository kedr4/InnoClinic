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

        builder.Services.AddApplicationServices(configuration);
        builder.Services.AddInfrastructureServices(configuration);
        builder.AddPresentationServices(configuration);

        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();

        var app = builder.Build();
        await app.SetupRolesAsync();

        app.ConfigureMiddlewares();
        app.MapControllers();

        app.Run();
    }
}
