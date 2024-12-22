using Application.Abstractions.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Application.Filters
{
    public class ValidateModelFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public ValidateModelFilter()
        {
            _logger = Log.ForContext<ValidateModelFilter>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value is IRequestDTO requestDTO)
                {
                    // Perform validation using the IRequestDTO's Validate method
                    var validationResult = requestDTO.Validate();

                    if (!validationResult.IsValid)
                    {
                        // Create a dictionary to hold validation errors
                        var validationErrors = validationResult.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(e => e.ErrorMessage).ToArray()
                            );

                        // Log validation errors
                        _logger.Warning("Validation failed for request: {ValidationErrors}", validationErrors);

                        // Add errors to ModelState for further processing by ASP.NET Core
                        foreach (var error in validationErrors)
                        {
                            foreach (var errorMessage in error.Value)
                            {
                                context.ModelState.AddModelError(error.Key, errorMessage);
                            }
                        }

                        // Set the response to return BadRequest with the validation details
                        var problemDetails = new ValidationProblemDetails(validationErrors)
                        {
                            Status = 400,
                            Title = "One or more validation errors occurred.",
                            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
                        };

                        // Interrupt the request pipeline and return the validation error response
                        context.Result = new BadRequestObjectResult(problemDetails);
                        return;
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
