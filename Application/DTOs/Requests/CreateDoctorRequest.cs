namespace Application.DTOs.Requests;

public record CreateDoctorRequest(

    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string MiddleName,
    bool IsLinkedToAccount = false,
    DateTimeOffset DateOfBirth = default,
    Guid SpecializationId = default,
    Guid OfficeId = default,
    int CareerStartYear = default

    );