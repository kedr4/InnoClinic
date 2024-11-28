namespace Application.DTOs.Requests;

public record CreateReceptionistRequest(

    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string MiddleName,
    DateTimeOffset DateOfBirth = default,
    Guid AccountId = default,
    Guid OfficeId = default

);
