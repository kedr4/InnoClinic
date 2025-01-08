using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Filters;

public class ValidationFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var actionArguments = context.ActionArguments;

        foreach (var argument in actionArguments.Values)
        {
            if (argument is not null)
            {
                var argumentType = argument.GetType();
                var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
                var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

                if (validator is not null)
                {
                    var validationResult = validator.Validate(new ValidationContext<object>(argument));

                    if (!validationResult.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(validationResult.Errors.Select(e => new
                        {
                            e.PropertyName,
                            e.ErrorMessage
                        }));

                        return;
                    }
                }
            }
        }
    }

}
