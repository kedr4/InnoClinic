using Application.Exceptions;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace Presentation.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An unhandled exception occurred."); 
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is AppException appException)
        {
            context.Response.StatusCode = appException.StatusCode;
            Log.Warning("Handled AppException: {Message}", appException.Message); 
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Log.Error("Unhandled Exception: {Message}", exception.Message); 
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
