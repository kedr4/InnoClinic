using FluentValidation;

namespace Business.Offices.Queries.GetOfficeById;

public class GetOfficeByIdQueryValidator : AbstractValidator<GetOfficeByIdQuery>
{
    public GetOfficeByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty().WithMessage("Office Id must be provided.");
    }
}
