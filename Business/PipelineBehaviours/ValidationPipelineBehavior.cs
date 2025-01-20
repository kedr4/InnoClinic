using Business.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Business.PipelineBehaviors;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationPipelineBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellation)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var validationFailures = validationResult.Errors.Select(e => new ValidationFailure(e.PropertyName, e.ErrorMessage)).ToList();

            throw new ValidationAppException(validationFailures);
        }

        return next();
    }
}
