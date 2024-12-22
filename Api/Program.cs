using Infrastructure;
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

        builder.Services.AddApplicationServices(configuration);
        builder.Services.AddInfrastructureServices(configuration);
        builder.AddPresentationServices(configuration);

        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseSerilogRequestLogging();
        RoleSetup.SetupRolesAsync(app);
        MiddlewareConfiguration.ConfigureMiddleware(app);

        app.MapControllers();

        app.Run();
    }
}
