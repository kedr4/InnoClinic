using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Abstractions.Validators;

public class UpdateDoctorRequestValidator : AbstractValidator<UpdateDoctorRequest>
{
    public UpdateDoctorRequestValidator()
    {
        // Email validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        // FirstName validation
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        // LastName validation
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        // Date of birth validation
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must(BeAValidAge).WithMessage("Doctor must be at least 18 years old.");

        // SpecializationId validation
        RuleFor(x => x.SpecializationId)
            .NotEmpty().WithMessage("Specialization ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Specialization ID must be a valid GUID.");

        // OfficeId validation
        RuleFor(x => x.OfficeId)
            .NotEmpty().WithMessage("Office ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Office ID must be a valid GUID.");


        // CareerStartYear validation
        RuleFor(x => x.CareerStartYear)
            .GreaterThanOrEqualTo(1900).WithMessage("Career start year must not be earlier than 1900.")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Career start year must not be in the future.");
    }

    private bool BeAValidAge(DateTimeOffset dateOfBirth)
    {
        var age = DateTimeOffset.Now.Year - dateOfBirth.Year;
        if (dateOfBirth.AddYears(age) > DateTimeOffset.Now)
        {
            age--;
        }
        return age >= 18;
    }

}
