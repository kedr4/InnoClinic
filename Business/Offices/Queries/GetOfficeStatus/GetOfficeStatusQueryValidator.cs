using FluentValidation;

namespace Business.Offices.Queries.GetOfficeStatus;

public class GetOfficeStatusQueryValidator : AbstractValidator<GetOfficeStatusQuery>
{
    public GetOfficeStatusQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty().WithMessage("Id must be provided.");
    }
}
