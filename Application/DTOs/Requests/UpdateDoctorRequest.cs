namespace Application.DTOs.Requests;

public record UpdateDoctorRequest(
    string Email,
    string FirstName,
    string LastName,
    string MiddleName,
    DateTimeOffset DateOfBirth = default,
    Guid SpecializationId = default,
    Guid OfficeId = default,
    int CareerStartYear = default
    );