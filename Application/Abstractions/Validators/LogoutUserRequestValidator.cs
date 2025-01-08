using Application.Abstractions.DTOs;
using FluentValidation;

namespace Application.Abstractions.Validators;

public class LogoutUserRequestValidator : AbstractValidator<LogoutUserRequest>
{
    public LogoutUserRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty().
            WithMessage("RefreshToken is required");
    }
}
