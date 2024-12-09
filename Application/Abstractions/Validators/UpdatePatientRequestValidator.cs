using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Validators;

public class UpdatePatientRequestValidator : AbstractValidator<UpdatePatientRequest>
{
    public UpdatePatientRequestValidator()
    {
        // Email validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        // First name validation
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        // Last name validation
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        // Middle name validation (optional)
        RuleFor(x => x.MiddleName)
            .MaximumLength(50).WithMessage("Middle name cannot exceed 50 characters.");

        // IsLinkedToAccount validation
        RuleFor(x => x.IsLinkedToAccount)
            .NotNull().WithMessage("IsLinkedToAccount must be specified.");

        // Date of Birth validation
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(BeAValidAge).WithMessage("Date of birth must indicate an age of at least 18 years.");
    }

    private bool BeAValidAge(DateTimeOffset dateOfBirth)
    {
        var age = DateTimeOffset.Now.Year - dateOfBirth.Year;
        if (dateOfBirth.AddYears(age) > DateTimeOffset.Now)
            age--;
        return age >= 18;
    }
}
