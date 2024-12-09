using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Abstractions.Validators;

public class DeleteProfileRequestValidator : AbstractValidator<DeleteProfileRequest>
{
    public DeleteProfileRequestValidator()
    {
        // UserId validation
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID.");

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
