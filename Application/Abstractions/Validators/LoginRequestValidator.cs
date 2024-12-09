using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Abstractions.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        // Email validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        // Password validation
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(15).WithMessage("Password must not exceed 15 characters.");

        // Role validation
        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Role must be a valid value.");
    }

}
