using FluentValidation;

namespace Business.Offices.Queries.GetAllOffices;

public class GetAllOfficesQueryValidator : AbstractValidator<GetAllOfficesQuery>
{
    public GetAllOfficesQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(query => query.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than 0.");

        RuleFor(query => query.IsActive)
            .Must(status => status == null || status == true || status == false)
            .WithMessage("IsActive must be a valid boolean value.");
    }
}
