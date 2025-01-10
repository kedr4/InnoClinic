using FluentValidation.Results;

namespace Business.Exceptions;

public class ValidationAppException : AppException
{
    public List<ValidationFailure> ValidationFailures { get; }

    public ValidationAppException(List<ValidationFailure> validationFailures)
        : base("Validation failed", 400)
    {
        ValidationFailures = validationFailures;
    }
}