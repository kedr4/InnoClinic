namespace Application.DTOs.Requests;

public record UpdateDoctorRequest
(
    string Email,
    string FirstName,
    string LastName,
    string MiddleName,
    DateTimeOffset DateOfBirth,
    Guid SpecializationId,
    Guid OfficeId,
    int CareerStartYear 
);