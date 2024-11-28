namespace Application.DTOs.Requests;

public record UpdatePatientRequest(
    string Email,
    string FirstName,
    string LastName,
    string MiddleName,
    bool IsLinkedToAccount = false,
    DateTimeOffset DateOfBirth = default
    );
