using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Linq;
using System.Collections.Generic;

namespace Application.Filters;

public class ValidateModelFilter : ActionFilterAttribute
{
    private readonly ILogger _logger;

    public ValidateModelFilter()
    {
        _logger = Log.ForContext<ValidateModelFilter>(); 
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var validationErrors = new Dictionary<string, string[]>();

            foreach (var state in context.ModelState)
            {
                if (state.Value?.Errors.Count > 0)
                {
                    validationErrors.Add(state.Key, state.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                }
            }

            _logger.Warning("Validation failed for request: {ValidationErrors}", validationErrors);

            var problemDetails = new ValidationProblemDetails(validationErrors)
            {
                Status = 400,
                Title = "One or more validation errors occurred.",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
            };

            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}
