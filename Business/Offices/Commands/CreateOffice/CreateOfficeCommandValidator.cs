using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Business.Offices.Commands.CreateOffice;

public class CreateOfficeCommandValidator : AbstractValidator<CreateOfficeCommand>
{
    private readonly string[] _allowedContentTypes = { "image/jpeg", "image/png" };

    public CreateOfficeCommandValidator()
    {
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.");

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required.");

        RuleFor(x => x.HouseNumber)
            .NotEmpty()
            .WithMessage("House number is required.");

        RuleFor(x => x.RegistryPhoneNumber)
            .NotEmpty()
            .WithMessage("Registry phone number is required.")
            .Matches(@"^\+\d+$")
            .WithMessage("You've entered an invalid phone number.");

        RuleFor(x => x.IsActive)
            .NotNull()
            .WithMessage("IsActive cannot be null.");

        RuleFor(x => x.Photo)
            .NotNull().WithMessage("Photo is required.")
            .Must(IsValidType).WithMessage("Only JPEG and PNG files are allowed.");

    }

    private bool IsValidType(IFormFile? file)
    {
        if (file is null)
        {
            return false;
        }

        return _allowedContentTypes.Contains(file.ContentType);
    }
}
