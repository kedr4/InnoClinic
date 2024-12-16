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

        var app = builder.Build();


        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Redirect the default route to the Swagger UI
        app.MapGet("/", () => Results.Redirect("/swagger"));

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseMiddlewares();

        app.Run();
    }
}
