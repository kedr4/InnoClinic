namespace Application.DTOs.Requests;

public record CreateDoctorRequest
(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string MiddleName,
    bool IsLinkedToAccount,
    DateTimeOffset DateOfBirth,
    Guid SpecializationId,
    Guid OfficeId,
    int CareerStartYear
);