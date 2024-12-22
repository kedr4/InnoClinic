public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        logger.LogInformation("Request started: {Method} {Url}", request.Method, request.Path);

        await next(context);

        logger.LogInformation("Request finished: {Method} {Url} {StatusCode}", request.Method, request.Path, context.Response.StatusCode);
    }
}
