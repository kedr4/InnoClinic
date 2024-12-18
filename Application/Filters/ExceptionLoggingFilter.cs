using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Application.Filters;

public class ExceptionLoggingFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionLoggingFilter> _logger;

    public ExceptionLoggingFilter(ILogger<ExceptionLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        Log.Error(context.Exception, "An error occurred while processing the request.");

        context.Result = new ObjectResult("An internal server error occurred.")
        {
            StatusCode = 500
        };
    }
}
