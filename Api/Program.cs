using Presentation.Middleware;
using Serilog;

namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IConfiguration configuration = builder.Configuration;

        builder.Services.AddProgramOptions(configuration);
        builder.Services.AddSwagger();
        builder.Services.AddControllers();
        builder.Services.AddEntityFramework(configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddAuthenticationServices();
        builder.Services.AddServices();
        builder.Services.AddValidation();
        builder.Services.AddOpenApi();
        builder.Services.AddSerilog(configuration);
        builder.Host.UseSerilog();

        var app = builder.Build();
        app.UseCustomExceptionHandlingMiddleware();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapGet("/", () => Results.Redirect("/swagger"));

        }
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSerilogRequestLogging();


        app.MapControllers();

        app.Run();
    }
}
