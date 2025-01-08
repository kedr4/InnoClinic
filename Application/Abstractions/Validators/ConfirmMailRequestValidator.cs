using Application.Abstractions.DTOs;
using FluentValidation;

namespace Application.Abstractions.Validators;

public class ConfirmMailRequestValidator : AbstractValidator<ConfirmMailRequest>
{
    public ConfirmMailRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Code is required");
    }
}
