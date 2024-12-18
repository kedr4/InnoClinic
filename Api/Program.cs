using Application.Filters;
using Infrastructure;
using Serilog;

namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddValidation();

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidateModelFilter>();
            options.Filters.Add<ExceptionLoggingFilter>();
        });

        IConfiguration configuration = builder.Configuration;

        builder.Services.AddProgramOptions(configuration);
        builder.Services.AddSwagger();
        builder.Services.AddControllers();
        builder.Services.AddEntityFramework(configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddAuthenticationServices();
        builder.Services.AddServices();
        builder.Services.AddOpenApi();

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

        app.MapControllers();

        app.Run();
    }
}
