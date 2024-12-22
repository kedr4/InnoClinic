using Presentation.Middleware;

namespace Presentation;

public static class MiddlewareConfiguration
{
    public static void ConfigureMiddleware(WebApplication app)
    {
        app.UseCustomExceptionHandlingMiddleware();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapGet("/", () => Results.Redirect("/swagger"));
        }

        app.UseApplicationSerilog(app.Configuration);
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<RequestResponseLoggingMiddleware>();
    }
}
