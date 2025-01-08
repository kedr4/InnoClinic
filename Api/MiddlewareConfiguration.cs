using Presentation.Middleware;

namespace Presentation;

public static class MiddlewareConfiguration
{
    public static void ConfigureMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();
        app.UseMiddleware<RequestResponseLoggingMiddleware>();

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
    }
}
