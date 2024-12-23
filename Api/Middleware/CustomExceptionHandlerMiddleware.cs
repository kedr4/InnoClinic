using Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Presentation.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogInformation(exception, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is AppException appException)
        {
            context.Response.StatusCode = appException.StatusCode;
            _logger.LogInformation("Handled AppException: {Message}", appException.Message);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.LogCritical("Unhandled Exception: {Message}", exception.Message);
        }

        var errorDetails = new
        {
            message = exception.Message,
            statusCode = context.Response.StatusCode
        };

        var result = JsonConvert.SerializeObject(errorDetails);
        return context.Response.WriteAsync(result);
    }
}
