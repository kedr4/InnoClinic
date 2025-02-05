using Business;
using Business.Abstractions.Services;
using DataAccess;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Presentation.Filters;
using Presentation.Middleware;
using Serilog;

namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DocumentsService", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and your token in the field below. \r\n\r\n Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
        });

        builder.Services.AddControllers();
        builder.Services.AddAuthorization();

        builder.Services.AddOpenApi();

        builder.Services.AddDataAccess(configuration);
        builder.Services.AddBusiness(configuration);

        builder.Services.AddHangfire(config =>
             config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddHangfireServer();

        var app = builder.Build();
        app.UseSerilogRequestLogging();
        app.UseMiddleware<GlobalExceptionHandler>();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseHangfireDashboard("/dashboard", new DashboardOptions { Authorization = new[] { new AllowAnonymousAuthorizationFilter() } });
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => Results.Redirect("/swagger"));
        app.MapControllers();

        //apply migrations
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DocumentsDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        app.Lifetime.ApplicationStarted.Register(() =>
        {
            using var scope = app.Services.CreateScope();
            var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            recurringJobManager.AddOrUpdate(
                "cleanup-job",
                () => scope.ServiceProvider.GetRequiredService<ICleanupService>().CleanupAsync(CancellationToken.None),
               Cron.Minutely);

        });

        app.Run();
    }
}
