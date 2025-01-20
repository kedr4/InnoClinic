using FluentValidation;

namespace Business.Offices.Commands.CreateOffice;

public class CreateOfficeCommandValidator : AbstractValidator<CreateOfficeCommand>
{
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
    }
}
