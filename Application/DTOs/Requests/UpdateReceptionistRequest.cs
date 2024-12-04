namespace Application.DTOs.Requests;

public record UpdateReceptionistRequest
(
    string FirstName,
    string LastName,
    string MiddleName,
    DateTimeOffset DateOfBirth,
    Guid AccountId,
    Guid OfficeId
);
