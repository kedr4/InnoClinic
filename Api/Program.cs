using Application.Filters;
using Application.Options;
using Infrastructure;
using Serilog;
using System.Reflection;

namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddValidation();
        builder.Configuration.AddApplicationUserSecrets();
        builder.Services.AddApplicationOptions();
        builder.Configuration.AddInfrastructureUserSecrets();
        builder.Services.AddInfrastructureOptions();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddScoped<ValidateModelFilter>();

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidateModelFilter>();
        });

        IConfiguration configuration = builder.Configuration;

        builder.Services.AddSwagger();
        builder.Services.AddControllers();

        builder.Services.AddEntityFramework(configuration);
        builder.Services.AddIdentityServices();
        builder.Services.AddAuthorization();


        builder.Services.AddAuthenticationServices(configuration);
        builder.Services.AddServices();
        builder.Services.AddRepositories();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseCustomExceptionHandlingMiddleware();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapGet("/", () => Results.Redirect("/swagger"));

        }
        //app.UseSerilogRequestLogging();
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<RequestResponseLoggingMiddleware>();


        app.MapControllers();

        app.Run();
    }
}
