using Business.Exceptions;
using Newtonsoft.Json;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Presentation.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger logger)
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
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is AppException appException)
        {
            context.Response.StatusCode = appException.StatusCode;

            var errorDetails = new
            {
                message = appException.Message,
                statusCode = context.Response.StatusCode,
            };

            var result = JsonConvert.SerializeObject(errorDetails);
            _logger.Error(result);

            return context.Response.WriteAsync(result);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var errorDetails = new
            {
                message = exception.Message,
                statusCode = context.Response.StatusCode
            };

            var result = JsonConvert.SerializeObject(errorDetails);
            _logger.Error(result);

            return context.Response.WriteAsync(result);
        }
    }
}
