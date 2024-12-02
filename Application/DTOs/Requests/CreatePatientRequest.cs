namespace Application.DTOs.Requests;

public record CreatePatientRequest
(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string MiddleName,
    bool IsLinkedToAccount = false,
    DateTimeOffset DateOfBirth = default,
    Guid AccountId = default
);
