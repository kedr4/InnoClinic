using Presentation.Middleware;

namespace Presentation;

public static class ProgramExceptionHandlingMiddleware
{
    public static IApplicationBuilder UseCustomExceptionHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        return app;
    }
}