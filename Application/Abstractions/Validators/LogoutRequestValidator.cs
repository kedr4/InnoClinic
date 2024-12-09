using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Abstractions.Validators;

public class LogoutRequestValidator : AbstractValidator<LogoutRequest>
{
    public LogoutRequestValidator()
    {
        // UserId validation
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID.");

        // RefreshToken validation
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .MinimumLength(32).WithMessage("Refresh token must be at least 32 characters long.");
    }
}
