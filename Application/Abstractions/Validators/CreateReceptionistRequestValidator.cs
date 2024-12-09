using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Abstractions.Validators;

public class CreateReceptionistRequestValidator : AbstractValidator<CreateReceptionistRequest>
{
    public CreateReceptionistRequestValidator()
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

        // ConfirmPassword matches Password
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required.")
            .Equal(x => x.Password).WithMessage("Passwords do not match.");

        // Name validations
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.MiddleName)
            .MaximumLength(50).WithMessage("Middle name cannot exceed 50 characters.");

        // Date of Birth validation
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(BeAValidAge).WithMessage("Receptionist must be at least 18 years old.");

        // AccountId validation
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Account ID must be a valid GUID.");

        // OfficeId validation
        RuleFor(x => x.OfficeId)
            .NotEmpty().WithMessage("Office ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Office ID must be a valid GUID.");
    }

    private bool BeAValidAge(DateTimeOffset dateOfBirth)
    {
        var age = DateTimeOffset.Now.Year - dateOfBirth.Year;
        if (dateOfBirth.AddYears(age) > DateTimeOffset.Now)
            age--;
        return age >= 18;
    }
}
