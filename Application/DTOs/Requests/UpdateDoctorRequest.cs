namespace Application.DTOs.Requests;

public record UpdateDoctorRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string MiddleName { get; init; } = string.Empty;
    public DateTimeOffset DateOfBirth { get; init; }
    public Guid SpecializationId { get; init; }
    public Guid OfficeId { get; init; }
    public int CareerStartYear { get; init; }

    public UpdateDoctorRequest(string firstName, string lastName, string middleName, DateTimeOffset dateOfBirth,
                               Guid specializationId, Guid officeId, int careerStartYear)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        SpecializationId = specializationId;
        OfficeId = officeId;
        CareerStartYear = careerStartYear;
    }
}
