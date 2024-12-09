using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Abstractions.Validators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        // Old Password validation
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password is required.")
            .MinimumLength(6).WithMessage("Old password must be at least 6 characters long.")
            .MaximumLength(15).WithMessage("Old password must not exceed 15 characters.");

        // New Password validation
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(6).WithMessage("New password must be at least 6 characters long.")
            .MaximumLength(15).WithMessage("New password must not exceed 15 characters.")
            .NotEqual(x => x.OldPassword).WithMessage("New password must be different from the old password.");
    }
}