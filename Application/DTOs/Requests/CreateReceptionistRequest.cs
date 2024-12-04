namespace Application.DTOs.Requests;

public record CreateReceptionistRequest
(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string MiddleName,
    DateTimeOffset DateOfBirth,
    Guid AccountId,
    Guid OfficeId
);
