using Application.Abstractions.DTOs;
using FluentValidation;

namespace Application.Abstractions.Validators;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.refreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");

        RuleFor(x => x.userId)
            .NotEmpty()
            .WithMessage("User ID is required.");
    }
}
