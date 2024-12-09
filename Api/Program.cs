namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IConfiguration configuration = builder.Configuration;

        builder.Services.AddSwagger();
        builder.Services.AddControllers();
        builder.Services.AddEntityFramework(configuration);
        builder.Services.AddAuthenticationServices(configuration);
        builder.Services.AddServices();

        builder.Services.AddAuthorization();
        builder.Services.AddValidation();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapControllers();   
        app.UseSwagger(app);
        app.UseMiddlewares();

        app.UseHttpsRedirection();
        app.UseAuthentication();  
        app.UseAuthorization();

        app.Run();
    }
}
