using FluentValidation;

namespace Business.Offices.Commands.ChangeOfficeStatus;

public class ChangeOfficeStatusCommandValidator : AbstractValidator<ChangeOfficeStatusCommand>
{
    public ChangeOfficeStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

        RuleFor(x => x.IsActive)
            .NotNull()
            .WithMessage("isActive is required.");
    }
}
